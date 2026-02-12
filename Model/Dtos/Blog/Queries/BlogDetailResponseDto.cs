using Core.Model;
using Model.Dtos.BlogComment.Queries;

namespace Model.Dtos.Blog.Queries;

public class BlogDetailResponseDto : IDto
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorFirstName { get; set; } = null!;
    public string AuthorLastName { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string BannerImage { get; set; } = null!;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public DateTime Date { get; set; }
    public List<BlogCommentDetailResponseDto>? CommentList { get; set; }
}

