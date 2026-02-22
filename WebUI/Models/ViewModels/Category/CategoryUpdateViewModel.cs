using Model.Dtos.Category.Commands;

namespace WebUI.Models.ViewModels.Category
{
    public class CategoryUpdateViewModel
    {
        // update dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public CategoryUpdateDto UpdateModel { get; set; } = new CategoryUpdateDto();
    }
}
