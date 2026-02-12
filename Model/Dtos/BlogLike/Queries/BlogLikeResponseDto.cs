using Core.Model;

namespace Model.Dtos.BlogLike.Queries;

public class BlogLikeResponseDto : IDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public string UserFirstName { get; set; } = null!;
    public string UserLastName { get; set; } = null!;
}