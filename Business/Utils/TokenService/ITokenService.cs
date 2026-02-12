using Core.Utils.Auth;
using Core.Utils.ResultPattern;
using Model.Entities;
using System.Security.Claims;

namespace Business.Utils.TokenService;

public interface ITokenService
{
    Result<AccessToken> GenerateAccessToken(IList<Claim> claims);
    Result<RefreshToken> GenerateRefreshToken(User user, string tokenValue, string clientType, Guid? deviceId = default);
    string GenerateRandomNumber();
    string HashToken(string token);
}