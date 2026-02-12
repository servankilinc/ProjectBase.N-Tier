using Core.Model;
using FluentValidation;

namespace Model.Dtos.Category.Commands;

public class CategoryCreateDto : IDto
{
    public string Name { get; set; } = null!;
}

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(v => v.Name).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Name).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Name).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
        RuleFor(v => v.Name).MaximumLength(20).WithMessage("Field must have a maximum 20 character");
    }
}
