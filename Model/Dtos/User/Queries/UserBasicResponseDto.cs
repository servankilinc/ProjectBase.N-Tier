using Core.Model;

namespace Model.Dtos.User.Commands;

public class UserBasicResponseDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
