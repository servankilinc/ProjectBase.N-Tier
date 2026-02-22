using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog.Commands;

namespace WebUI.Models.ViewModels.Blog
{
    public class BlogCreateViewModel
    {
        // create dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public BlogCreateDto CreateModel { get; set; } = new BlogCreateDto();

        public SelectList? CategoryIds { get; set; }
        public SelectList? AuthorIds { get; set; }
    }
}
