using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

public class BlogLikeRepository : RepositoryBase<BlogLike, AppDbContext>, IBlogLikeRepository
{
    public BlogLikeRepository(AppDbContext context) : base(context)
    {
    }
}
