using DataAccess.Repository;
using Model.ProjectEntities;

namespace DataAccess.Abstract;

public interface ILocalizationRepository : IRepository<Localization>, IRepositoryAsync<Localization>
{
}
