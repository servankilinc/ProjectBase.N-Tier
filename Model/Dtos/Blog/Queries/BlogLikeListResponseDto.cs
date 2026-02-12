using Core.Model;
using Model.Dtos.User.Commands;

namespace Model.Dtos.Blog.Queries;

public class BlogLikeListResponseDto : IDto
{
    public Guid BlogId { get; set; }
    public int LikeCount { get; set; }
    public List<UserBasicResponseDto>? UserList { get; set; }
}
