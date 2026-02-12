using Core.Model;
using FluentValidation;

namespace Model.Dtos.User.Queries;

public class UserCreateDto : IDto
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Addres { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string Password { get; set; } = null!;
}

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(v => v.Name).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Name).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Name).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
        RuleFor(v => v.Name).MaximumLength(30).WithMessage("Field must have a maximum 30 character");

        RuleFor(v => v.LastName).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.LastName).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.LastName).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
        RuleFor(v => v.LastName).MaximumLength(30).WithMessage("Field must have a maximum 30 character");

        RuleFor(v => v.Email).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Email).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Email).EmailAddress().WithMessage("Field must be a valid email address");
    
        RuleFor(v => v.Password).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Password).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Password).MinimumLength(6).WithMessage("Field must have a minimum 6 character");
    }
}
