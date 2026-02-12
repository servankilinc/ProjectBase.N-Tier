using Core.Model;

namespace Model.Dtos.Blog.Queries;

public class BlogReportDto : IDto
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string? AuthorName { get; set; } // AuthorId kut (Id) + Name => AuthorName
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; } // CategoryId kut (Id) + Name => CategoryName
    public DateTime Date { get; set; }
    public string BannerImage { get; set; } = null!;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }

    // entity auditable ise
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }
    // Soft Deletable ise
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateUtc { get; set; }
}
