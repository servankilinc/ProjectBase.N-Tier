using Core.Model;
using Model.Dtos.Blog.Queries;

namespace Model.Dtos.Category.Queries;

public class CategoryBlogsResponseDto : IDto
{
    public CategoryResponseDto? Category { get; set; }
    public List<BlogBasicResponseDto>? BlogList { get; set; }
}