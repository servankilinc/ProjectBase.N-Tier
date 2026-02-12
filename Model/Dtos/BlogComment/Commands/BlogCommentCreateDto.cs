using Core.Model;
using FluentValidation;

namespace Model.Dtos.BlogComment.Commands;

public class BlogCommentCreateDto : IDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; } = null!;
}

public class BlogCommentCreateDtoValidator : AbstractValidator<BlogCommentCreateDto>
{
    public BlogCommentCreateDtoValidator()
    {
        RuleFor(v => v.BlogId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.BlogId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");

        RuleFor(v => v.UserId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.UserId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");

        RuleFor(v => v.Comment).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Comment).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Comment).MinimumLength(2).WithMessage("Field must have a minimum 2 character");
    }
}
