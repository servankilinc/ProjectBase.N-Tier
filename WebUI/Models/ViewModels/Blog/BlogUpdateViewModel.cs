using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog.Commands;

namespace WebUI.Models.ViewModels.Blog
{
    public class BlogUpdateViewModel
    {
        // update dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public BlogUpdateDto UpdateModel { get; set; } = new BlogUpdateDto();

        public SelectList? CategoryIds { get; set; }
        public SelectList? AuthorIds { get; set; }
    }
}
