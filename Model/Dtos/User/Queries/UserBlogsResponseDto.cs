using Core.Model;
using Model.Dtos.Blog.Queries;

namespace Model.Dtos.User.Commands;

public class UserBlogsResponseDto : IDto
{
    public UserBasicResponseDto? User { get; set; }
    public List<BlogBasicResponseDto>? BlogList { get; set; }
}
