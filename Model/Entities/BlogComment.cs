using Core.Model;

namespace Model.Entities;

public class BlogComment : IEntity, IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime Date { get; set; }

    #region Relations
    public Blog? Blog { get; set; }
    public User? User { get; set; }
    #endregion

    #region Legacy Props
    // Auditable Properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }
    #endregion
}
