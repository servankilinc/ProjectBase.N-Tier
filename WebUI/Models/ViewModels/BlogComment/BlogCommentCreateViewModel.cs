using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.BlogComment.Commands;

namespace WebUI.Models.ViewModels.BlogComment
{
    public class BlogCommentCreateViewModel
    {
        // create dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public BlogCommentCreateDto CreateModel { get; set; } = new BlogCommentCreateDto();

        public SelectList? BlogIds { get; set; }
        public SelectList? UserIds { get; set; }
    }
}
