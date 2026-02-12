using Core.Model;
using FluentValidation;

namespace Model.Dtos.BlogComment.Commands;

public class BlogCommentUpdateDto : IDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = null!;
}
public class BlogCommentUpdateDtoValidator : AbstractValidator<BlogCommentUpdateDto>
{
    public BlogCommentUpdateDtoValidator()
    {
        RuleFor(v => v.Id).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Id).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");

        RuleFor(v => v.Comment).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Comment).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Comment).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
    }
}
