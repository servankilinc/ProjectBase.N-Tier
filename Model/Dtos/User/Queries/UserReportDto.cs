using Core.Model;

namespace Model.Dtos.User.Commands;

public class UserReportDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Addres { get; set; }
    public DateOnly? BirthDate { get; set; }

    // entity auditable ise
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }

    // Soft Deletable ise
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateUtc { get; set; }
}
