using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

public class UserRepository : RepositoryBase<User, AppDbContext>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}