using DataAccess.Repository;
using Model.Entities;
using System.Linq.Expressions;

namespace DataAccess.Abstract;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IRepositoryAsync<RefreshToken>
{
    public void RevokeDeviceRefreshTokens(Expression<Func<RefreshToken, bool>> where);
    public Task RevokeDeviceRefreshTokensAsync(Expression<Func<RefreshToken, bool>> where, CancellationToken cancellationToken = default);
}
