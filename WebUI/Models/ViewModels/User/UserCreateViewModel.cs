using Model.Dtos.User.Commands;

namespace WebUI.Models.ViewModels.User
{
    public class UserCreateViewModel
    {
        // create dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public UserCreateDto CreateModel { get; set; } = new UserCreateDto();
    }
}
