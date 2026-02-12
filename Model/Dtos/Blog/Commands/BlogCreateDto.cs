using Core.Model;
using FluentValidation;

namespace Model.Dtos.Blog.Commands;

public class BlogCreateDto : IDto
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string BannerImage { get; set; } = null!;
}


public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
{
    public BlogCreateDtoValidator()
    {
        RuleFor(v => v.AuthorId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.AuthorId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");
        
        RuleFor(v => v.Title).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Title).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Title).MinimumLength(6).WithMessage("Field must have a minimum 6 character");
        RuleFor(v => v.Title).MaximumLength(80).WithMessage("Field must have a maximum 80 character");

        RuleFor(v => v.Content).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Content).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Content).MinimumLength(20).WithMessage("Field must have a minimum 20 character");

        RuleFor(v => v.CategoryId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.CategoryId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");

        RuleFor(v => v.BannerImage).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.BannerImage).NotEmpty().WithMessage("Field cannot be empty");
    }
}