using Core.Model;

namespace Model.Entities;

public class BlogLike : IEntity
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }

    #region Relations
    public virtual Blog? Blog { get; set; }
    public virtual User? User { get; set; }
    #endregion
}