using Core.Model;

namespace Model.Dtos.User.Commands;

public class UserDetailResponseDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Addres { get; set; }
    public string? BirthDate { get; set; }
}
