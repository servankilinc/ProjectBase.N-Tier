using FluentValidation;

namespace Model.Auth.Refresh;

public class RefreshAuthRequest
{
    public Guid UserId { get; set; }
    public string? RefreshToken { get; set; }
    public Guid DeviceId { get; set; }
}

public class RefreshAuthRequestValidator : AbstractValidator<RefreshAuthRequest>
{
    public RefreshAuthRequestValidator()
    {
        RuleFor(b => b.UserId).NotNull().NotEqual(Guid.Empty).NotEmpty();
        RuleFor(b => b.DeviceId).NotNull().NotEqual(Guid.Empty).NotEmpty();
    }
}