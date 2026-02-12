using Core.Model;

namespace Model.Entities;

public class Blog: IEntity, ISoftDeletableEntity, IAuditableEntity, IArchivableEntity
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public DateTime Date { get; set; }
    public string BannerImage { get; set; } = null!;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }

    #region Relations
    public virtual User? Author { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<BlogLike>? BlogLikes { get; set; }
    public virtual ICollection<BlogComment>? BlogComments { get; set; }
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
