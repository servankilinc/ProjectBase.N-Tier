using Core.Model;

namespace Model.Dtos.BlogComment.Queries;

public class BlogCommentBasicResponseDto : IDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BlogId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime Date { get; set; }
}
