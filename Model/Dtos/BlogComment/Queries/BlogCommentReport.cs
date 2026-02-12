using Core.Model;

namespace Model.Dtos.BlogComment.Queries;

public class BlogCommentReportDto : IDto
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public string? BlogTitle { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime Date { get; set; }

    // entity auditable ise
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }
}
