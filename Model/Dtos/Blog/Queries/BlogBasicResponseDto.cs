using Core.Model;

namespace Model.Dtos.Blog.Queries;

public class BlogBasicResponseDto : IDto
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime Date { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string BannerImage { get; set; } = null!;
}
