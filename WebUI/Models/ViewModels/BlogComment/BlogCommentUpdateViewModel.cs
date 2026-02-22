using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.BlogComment.Commands;

namespace WebUI.Models.ViewModels.BlogComment
{
    public class BlogCommentUpdateViewModel
    {
        // update dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public BlogCommentUpdateDto UpdateModel { get; set; } = new BlogCommentUpdateDto();

        public SelectList? BlogIds { get; set; }
        public SelectList? UserIds { get; set; }
    }
}
