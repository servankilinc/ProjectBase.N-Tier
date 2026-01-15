using DataAccess.Repository;
using Model.ProjectEntities;

namespace DataAccess.Abstract;

public interface ILanguageRepository : IRepository<Language>, IRepositoryAsync<Language>
{
}