using Model.Dtos.Category.Commands;

namespace WebUI.Models.ViewModels.Category
{
    public class CategoryCreateViewModel
    {
        // create dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public CategoryCreateDto CreateModel { get; set; } = new CategoryCreateDto();
    }
}
