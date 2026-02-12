using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using System.Linq.Expressions;

namespace DataAccess.Concrete;

public class RefreshTokenRepository : RepositoryBase<RefreshToken, AppDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }

    public void RevokeDeviceRefreshTokens(Expression<Func<RefreshToken, bool>> where)
    {
        _context.RefreshTokens.Where(where).ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true));
    }

    public async Task RevokeDeviceRefreshTokensAsync(Expression<Func<RefreshToken, bool>> where, CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.Where(where).ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true), cancellationToken);
    }
}