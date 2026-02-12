using Core.Model;
using FluentValidation;

namespace Model.Dtos.BlogLike.Commands;

public class BlogLikeCreateDto : IDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
}

public class BlogLikeCreateDtoValidator : AbstractValidator<BlogLikeCreateDto>
{
    public BlogLikeCreateDtoValidator()
    {
        RuleFor(v => v.BlogId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.BlogId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");

        RuleFor(v => v.UserId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.UserId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");
    }
}
