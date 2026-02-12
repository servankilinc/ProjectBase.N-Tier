using Core.Model;

namespace Model.Dtos.BlogComment.Queries;

public class BlogCommentDetailResponseDto : IDto
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public string UserFirstName { get; set; } = null!;
    public string UserLastName { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public DateTime Date { get; set; }
}
