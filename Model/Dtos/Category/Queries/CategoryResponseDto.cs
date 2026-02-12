using Core.Model;

namespace Model.Dtos.Category.Queries;

public class CategoryResponseDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
