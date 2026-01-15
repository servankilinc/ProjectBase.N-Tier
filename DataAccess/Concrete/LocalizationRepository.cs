using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.ProjectEntities;

namespace DataAccess.Concrete;

[DataAccessException]
public class LocalizationRepository : RepositoryBase<Localization, AppDbContext>, ILocalizationRepository
{
    public LocalizationRepository(AppDbContext context) : base(context)
    {
    }
}
