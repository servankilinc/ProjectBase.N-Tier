using Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Model.Entities;

public class User : IdentityUser<Guid>, IEntity, ISoftDeletableEntity, IAuditableEntity, IArchivableEntity
{
    // Id Email gibi alanları Identity kütüphanesi sağlıyor yinede girilebilir
    public override Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public override string Email { get; set; } = null!;
    public string? Addres { get; set; }
    public DateOnly? BirthDate { get; set; }
    
    #region Relations
    public virtual ICollection<Blog>? Blogs { get; set; }
    public virtual ICollection<BlogLike>? BlogLikes { get; set; }
    public virtual ICollection<BlogComment>? BlogComments { get; set; }
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
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
