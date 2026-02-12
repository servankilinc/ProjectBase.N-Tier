using Core.Enums;
using Core.Utils.ResultPattern;

namespace Core.Utils.HttpContextManager
{
    public interface IHttpContextManager
    {
        Result<string> GetNameIdentifier();
        Result<string> GetUserAgent();
        Result<string> GetClientIp();
        Result<string> GetCurrentCulture();
        Result<byte> GetCurrentLanguageId();
        Result<Language> GetCurrentLanguage();
        Result SetCurrentCulture(string culture);
        Result<string> GetRefreshTokenFromCookie();
        Result AddRefreshTokenToCookie(string refreshToken, DateTime expirationUtc);
        Result DeletetRefreshTokenFromCookie();
    }
}