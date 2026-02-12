using Core.Model;

namespace Model.Entities;

public class Category : IEntity, ISoftDeletableEntity, IAuditableEntity, IArchivableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    #region Relations
    public virtual ICollection<Blog>? Blogs { get; set; }
    #endregion

    #region Legacy Props
    // Auditable Properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }

    // Soft Deletable Properties
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateUtc { get; set; }
    #endregion
}
