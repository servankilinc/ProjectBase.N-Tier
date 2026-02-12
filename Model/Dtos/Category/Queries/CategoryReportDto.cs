using Core.Model;

namespace Model.Dtos.Category.Queries;

public class CategoryReportDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    // entity auditable ise
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }

    // Soft Deletable ise
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateUtc { get; set; }
}
