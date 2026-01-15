using FluentValidation;

namespace Model.Auth.RefreshAuth;

public class RefreshAuthRequest
{
    public Guid UserId { get; set; }
    public bool IsTrusted { get; set; }
    public string RefreshToken { get; set; } = null!;
}

public class RefreshAuthRequestValidator : AbstractValidator<RefreshAuthRequest>
{
    public RefreshAuthRequestValidator()
    {
        RuleFor(b => b.UserId).NotNull().NotEqual(Guid.Empty).NotEmpty();
        When(b => !b.IsTrusted, () => RuleFor(b => b.RefreshToken).NotNull().NotEmpty());
    }
}