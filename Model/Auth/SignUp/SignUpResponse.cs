using Core.Utils.Auth;
using Model.Dtos.User.Commands;

namespace Model.Auth.SignUp;

public class SignUpResponse
{
    public IList<string>? Roles { get; set; }
    public AccessToken AccessToken { get; set; } = null!;
    public UserBasicResponseDto User { get; set; } = null!;
    public Guid DeviceId { get; set; }
}

public class SignUpTrustedResponse : SignUpResponse
{
    public string RefreshToken { get; set; } = null!;
}
