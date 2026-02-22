using Core.Model;
using FluentValidation;

namespace Model.Dtos.User.Commands;

public class UserUpdateDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Addres { get; set; }
    public DateOnly BirthDate { get; set; }
}

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(v => v.Name).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Name).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Name).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
        RuleFor(v => v.Name).MaximumLength(30).WithMessage("Field must have a maximum 30 character");

        RuleFor(v => v.LastName).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.LastName).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.LastName).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
        RuleFor(v => v.LastName).MaximumLength(30).WithMessage("Field must have a maximum 30 character");
    }
}
