using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.ProjectEntities;

namespace DataAccess.Concrete;

[DataAccessException]
public class LocalizationLanguageDetailRepository : RepositoryBase<LocalizationLanguageDetail, AppDbContext>, ILocalizationLanguageDetailRepository
{
    public LocalizationLanguageDetailRepository(AppDbContext context) : base(context)
    {
    }
}
