using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models.ViewModels.BlogComment
{
    public class BlogCommentViewModel
    {
        public SelectList? BlogIds { get; set; }
        public SelectList? UserIds { get; set; }
        public BlogCommentFilterModel FilterModel { get; set; } = new BlogCommentFilterModel();
    }

    public class BlogCommentFilterModel
    {
        // *** bu alanlar entity içindeki filterable fieldlardan getirilir ***
        public Guid BlogId { get; set; }
        public Guid UserId { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
