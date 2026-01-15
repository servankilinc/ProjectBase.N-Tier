using DataAccess.Repository;
using Model.ProjectEntities;

namespace DataAccess.Abstract;

public interface ILocalizationLanguageDetailRepository : IRepository<LocalizationLanguageDetail>, IRepositoryAsync<LocalizationLanguageDetail>
{
}