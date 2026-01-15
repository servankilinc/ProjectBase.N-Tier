using DataAccess.Abstract;

namespace DataAccess.UoW;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    #region Repositories
    IUserRepository Users { get; }
    IBlogRepository Blogs { get; }
    ICategoryRepository Categories { get; }
    IBlogCommentRepository BlogComments { get; }
    IBlogLikeRepository BlogLikes { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    ILanguageRepository Languages { get; }
    ILocalizationRepository Localizations { get; }
    ILocalizationLanguageDetailRepository LocalizationLanguageDetails { get; }
    #endregion


    int SaveChanges();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
