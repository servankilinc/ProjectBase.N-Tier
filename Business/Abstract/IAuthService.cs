using Core.Utils.ResultPattern;
using Model.Auth.Login;
using Model.Auth.Refresh;
using Model.Auth.SignUp;

namespace Business.Abstract;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    Task<Result<SignUpResponse>> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default);
    Task<Result<RefreshAuthResponse>> RefreshAuthAsync(RefreshAuthRequest refreshAuthRequest, CancellationToken cancellationToken = default);
}