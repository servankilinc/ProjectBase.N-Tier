using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.ProjectEntities;

namespace DataAccess.Concrete;

[DataAccessException]
public class LanguageRepository : RepositoryBase<Language, AppDbContext>, ILanguageRepository
{
    public LanguageRepository(AppDbContext context) : base(context)
    {
    }
}