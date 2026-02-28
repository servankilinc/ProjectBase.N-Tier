using AutoMapper;
using Business.Abstract;
using Business.Utils.TokenService;
using Core.Enums;
using Core.Utils.Auth;
using Core.Utils.HttpContextManager;
using Core.Utils.ResultPattern;
using Core.Utils.Validation;
using DataAccess.UoW;
using Microsoft.AspNetCore.Identity;
using Model.Auth.Login;
using Model.Auth.Refresh;
using Model.Auth.SignUp;
using Model.Dtos.User.Commands;
using Model.Entities;
using System.Security.Claims;
using static Core.Utils.GlobalExtensions;

namespace Business.Concrete;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextManager _httpContextManager;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IHttpContextManager httpContextManager,
        IValidationService validationService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextManager = httpContextManager;
        _validationService = validationService;
        _mapper = mapper;
    }


    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(loginRequest, cancellationToken);
        if (!validationResult.IsValid)
            return Result<LoginResponse>.Validation(validationResult.Failures);

        // 1) Find user by credentials
        User? user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
            return Result<LoginResponse>.Failure(message: "The email address or password was wrong.", metadata: Meta("Requester Email", loginRequest.Email));

        // 2) Check password
        SignInResult checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: true);
        if (!checkPassword.Succeeded)
        {
            if (checkPassword.IsLockedOut)
                return Result<LoginResponse>.Failure(message: "Your account is temporarily locked due to multiple failed login attempts.", metadata: Meta("Requester Email", loginRequest.Email));
            if (checkPassword.RequiresTwoFactor)
                return Result<LoginResponse>.Failure(message: "Two-factor authentication is required to login.", metadata: Meta("Requester Email", loginRequest.Email));
            if (checkPassword.IsNotAllowed)
                return Result<LoginResponse>.Failure(message: "The user is not allowed to sign in.", metadata: Meta("Requester Email", loginRequest.Email));
            return Result<LoginResponse>.Failure(message: "The email address or password was wrong.", metadata: Meta("Requester Email", loginRequest.Email));
        }

        if (!await _signInManager.CanSignInAsync(user))
        {
            return Result<LoginResponse>.Failure(message: "You are not allowed to login.", metadata: Meta("User", user));
        }

        // 3) Get user roles and claims
        IList<string> roles = await _userManager.GetRolesAsync(user);
        IList<Claim> claims = await GetClaimsAsync(user, roles);

        // 3) Generate Access Token and Refresh Token
        Result<AccessToken> accessToken = _tokenService.GenerateAccessToken(claims);
        if (!accessToken.IsSuccess)
            return Result<LoginResponse>.Failure(description: "Access token could not generated", metadata: Meta("Access Token Result", accessToken));


        string tokenValue = _tokenService.GenerateRandomNumber();
        Result<RefreshToken> refreshToken = _tokenService.GenerateRefreshToken(user, tokenValue, loginRequest.ClientType, loginRequest.DeviceId);
        if (!refreshToken.IsSuccess)
            return Result<LoginResponse>.Failure(description: "Refresh token could not generated", metadata: Meta("Refresh Token Result", refreshToken));

        // 4) Save Refresh Token and Revoke old ones if deviceId is provided
        if (loginRequest.DeviceId != null && loginRequest.DeviceId.HasValue)
        {
            await _unitOfWork.RefreshTokens.RevokeDeviceRefreshTokensAsync(f => f.DeviceId == loginRequest.DeviceId.Value && f.IsRevoked == false);
        }
        await _unitOfWork.RefreshTokens.AddAndSaveAsync(refreshToken.Data, cancellationToken);

        if (refreshToken.Data.ClientType != ClientType.Web)
        {
            return Result<LoginResponse>.Success(new LoginTrustedResponse
            {
                AccessToken = accessToken.Data,
                RefreshToken = tokenValue,
                DeviceId = refreshToken.Data.DeviceId,
                User = _mapper.Map<UserBasicResponseDto>(user),
                Roles = roles
            });
        }
        else
        {
            _httpContextManager.AddRefreshTokenToCookie(tokenValue, refreshToken.Data.ExpirationUtc);
            return Result<LoginResponse>.Success(new LoginResponse
            {
                AccessToken = accessToken.Data,
                DeviceId = refreshToken.Data.DeviceId,
                User = _mapper.Map<UserBasicResponseDto>(user),
                Roles = roles
            });
        }
    }


    public async Task<Result<SignUpResponse>> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validationService.ValidateAsync(signUpRequest, cancellationToken);
            if (!validationResult.IsValid)
                return Result<SignUpResponse>.Validation(validationResult.Failures);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // 1) Check if user already exists
            var userExist = await _userManager.FindByEmailAsync(signUpRequest.Email);
            if (userExist != null)
                return Result<SignUpResponse>.Failure(message: "The email address is already in use.", metadata: Meta("Request Email", signUpRequest.Email));

            // 2) Create new user
            var user = _mapper.Map<User>(signUpRequest);
            user.UserName = signUpRequest.Email;
            var result = await _userManager.CreateAsync(user, signUpRequest.Password);
            if (!result.Succeeded)
                return Result<SignUpResponse>.Failure(description: $"User cannot be created.", metadata: Meta(("Requester Email", signUpRequest.Email), ("Identity Service Errors", result)));

            // 3) Assign "User" role to the new user
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return Result<SignUpResponse>.Failure(description: $"Failed to assign role", metadata: Meta(("Requester Email", signUpRequest.Email), ("Identity Service Errors", roleResult)));

            // 4) Get user roles and claims
            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await GetClaimsAsync(user, roles);

            // 5) Generate Access Token and Refresh Token
            Result<AccessToken> accessToken = _tokenService.GenerateAccessToken(claims);
            if (!accessToken.IsSuccess)
                return Result<SignUpResponse>.Failure(description: "Access token could not generated", metadata: Meta("Access Token Result", accessToken));

            string tokenValue = _tokenService.GenerateRandomNumber();
            Result<RefreshToken> refreshToken = _tokenService.GenerateRefreshToken(user, tokenValue, signUpRequest.ClientType, signUpRequest.DeviceId);
            if (!refreshToken.IsSuccess)
                return Result<SignUpResponse>.Failure(description: "Refresh token could not generated", metadata: Meta("Refresh Token Result", refreshToken));

            // 6) Save Refresh Token and Revoke old ones if deviceId is provided
            if (signUpRequest.DeviceId != null && signUpRequest.DeviceId.HasValue)
            {
                await _unitOfWork.RefreshTokens.RevokeDeviceRefreshTokensAsync(f => f.DeviceId == signUpRequest.DeviceId.Value && f.IsRevoked == false);
            }
            await _unitOfWork.RefreshTokens.AddAndSaveAsync(refreshToken.Data, cancellationToken);


            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (signUpRequest.ClientType != ClientType.Web)
            {
                return Result<SignUpResponse>.Success(new SignUpTrustedResponse
                {
                    AccessToken = accessToken.Data,
                    RefreshToken = tokenValue,
                    DeviceId = refreshToken.Data.DeviceId,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                    Roles = roles,
                });
            }
            else
            {
                _httpContextManager.AddRefreshTokenToCookie(tokenValue, refreshToken.Data.ExpirationUtc);
                return Result<SignUpResponse>.Success(new SignUpResponse
                {
                    AccessToken = accessToken.Data,
                    DeviceId = refreshToken.Data.DeviceId,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                    Roles = roles,
                });
            }
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<RefreshAuthResponse>> RefreshAuthAsync(RefreshAuthRequest refreshAuthRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validationService.ValidateAsync(refreshAuthRequest, cancellationToken);
            if (!validationResult.IsValid)
                return Result<RefreshAuthResponse>.Validation(validationResult.Failures);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // 1) Set refresh token from cookie if not provided
            if (string.IsNullOrWhiteSpace(refreshAuthRequest.RefreshToken))
            {
                var cookieValue = _httpContextManager.GetRefreshTokenFromCookie();
                if (!cookieValue.IsSuccess)
                    return Result<RefreshAuthResponse>.Failure(description: "Refresh auth request cookie not found in cookie", metadata: Meta("Cookie Result", cookieValue.Error.Description));
                refreshAuthRequest.RefreshToken = cookieValue.Data;
            }
            string hashedToken = _tokenService.HashToken(refreshAuthRequest.RefreshToken);

            // 2) Find refresh token record
            RefreshToken? refreshToken = await _unitOfWork.RefreshTokens.GetAsync(where: f =>
                f.UserId == refreshAuthRequest.UserId &&
                f.DeviceId == refreshAuthRequest.DeviceId &&
                f.Token == hashedToken &&
                f.TTL > 0 &&
                f.IsRevoked == false &&
                f.ExpirationUtc > DateTime.UtcNow,
                cancellationToken: cancellationToken);
            if (refreshToken == null)
                return Result<RefreshAuthResponse>.Failure(description: "There is no refresh token that can be used.", metadata: Meta("Request Model", refreshAuthRequest));

            // 3) Find user
            var user = await _unitOfWork.Users.GetAsync(where: f => f.Id == refreshAuthRequest.UserId, cancellationToken: cancellationToken);
            if (user == null)
                return Result<RefreshAuthResponse>.Failure(description: $"User cannot found for refresh auth, userId: {refreshAuthRequest.UserId}", metadata: Meta("Request Model", refreshAuthRequest));

            // 4) Update refresh token 
            string tokenValue = _tokenService.GenerateRandomNumber();
            refreshToken.Token = _tokenService.HashToken(tokenValue);
            refreshToken.TTL -= 1;
            await _unitOfWork.RefreshTokens.UpdateAndSaveAsync(refreshToken, cancellationToken);

            // 5) revoke old tokens for the device
            await _unitOfWork.RefreshTokens.RevokeDeviceRefreshTokensAsync(f => f.DeviceId == refreshAuthRequest.DeviceId && f.IsRevoked == false && f.Id != refreshToken.Id, cancellationToken);

            // 6) Get user roles and claims
            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await GetClaimsAsync(user, roles);

            // 7) Generate new access token
            Result<AccessToken> accessToken = _tokenService.GenerateAccessToken(claims);
            if (!accessToken.IsSuccess)
                return Result<RefreshAuthResponse>.Failure(description: "Access token could not generated", metadata: Meta("Access Token Result", accessToken));

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (refreshToken.ClientType != ClientType.Web)
            {
                return Result<RefreshAuthResponse>.Success(new RefreshAuthTrustedResponse
                {
                    AccessToken = accessToken.Data,
                    RefreshToken = tokenValue,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                    Roles = roles
                });
            }
            else
            {
                _httpContextManager.AddRefreshTokenToCookie(tokenValue, refreshToken.ExpirationUtc);
                return Result<RefreshAuthResponse>.Success(new RefreshAuthResponse
                {
                    AccessToken = accessToken.Data,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                    Roles = roles
                });
            }
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }


    #region HELPERS
    private async Task<IList<Claim>> GetClaimsAsync(User user, IList<string> roles)
    {
        List<Claim> claimList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}")
        };

        if (!string.IsNullOrEmpty(user.Email))
            claimList.Add(new Claim(ClaimTypes.Email, user.Email));

        IList<Claim>? persistentClaims = await _userManager.GetClaimsAsync(user);
        claimList.AddRange(persistentClaims);

        IEnumerable<Claim>? roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
        claimList.AddRange(roleClaims);

        // password, role vs. değişdiğinde mevcut tokenları geçersiz kılmak için security stamp eklenebilir
        // var securityStamp = await _userManager.GetSecurityStampAsync(user);
        // claimList.Add(new Claim("app_security_stamp_claim", securityStamp));

        return claimList;
    }
    #endregion
}