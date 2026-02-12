using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

public class CategoryRepository : RepositoryBase<Category, AppDbContext>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
