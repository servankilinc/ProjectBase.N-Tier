using Core.Utils.Auth;
using Model.Dtos.User.Commands;

namespace Model.Auth.Refresh;

public class RefreshAuthResponse
{
    public IList<string>? Roles { get; set; }
    public UserBasicResponseDto User { get; set; } = null!;
    public AccessToken AccessToken { get; set; } = null!;
}

public class RefreshAuthTrustedResponse : RefreshAuthResponse
{
    public string RefreshToken { get; set; } = null!;
}