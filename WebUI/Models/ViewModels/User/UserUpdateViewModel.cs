using Model.Dtos.User.Commands;

namespace WebUI.Models.ViewModels.User
{
    public class UserUpdateViewModel
    {
        // Update dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public UserUpdateDto UpdateModel { get; set; } = new UserUpdateDto();
    }
}
