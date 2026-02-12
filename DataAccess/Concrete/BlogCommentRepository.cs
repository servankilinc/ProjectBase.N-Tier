using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

public class BlogCommentRepository : RepositoryBase<BlogComment, AppDbContext>, IBlogCommentRepository
{
    public BlogCommentRepository(AppDbContext context) : base(context)
    {
    }
}