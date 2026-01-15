using DataAccess.Abstract;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    #region Repositories
    public IUserRepository Users { get; private set; }
    public IBlogRepository Blogs { get; private set; }
    public ICategoryRepository Categories { get; private set; }
    public IBlogLikeRepository BlogLikes { get; private set; }
    public IBlogCommentRepository BlogComments { get; private set; }
    public IRefreshTokenRepository RefreshTokens { get; private set; }
    public ILanguageRepository Languages { get; private set; }
    public ILocalizationRepository Localizations { get; private set; }
    public ILocalizationLanguageDetailRepository LocalizationLanguageDetails { get; private set; }
    #endregion


    public UnitOfWork(
        AppDbContext context,
        IUserRepository userRepository,
        IBlogRepository blogRepository,
        ICategoryRepository categoryRepository,
        IBlogLikeRepository blogLikeRepository,
        IBlogCommentRepository blogCommentRepository,
        IRefreshTokenRepository refreshTokens,
        ILanguageRepository languages,
        ILocalizationRepository localizations,
        ILocalizationLanguageDetailRepository localizationLanguageDetails
    ){
        _context = context;
        Users = userRepository;
        Blogs = blogRepository;
        Categories = categoryRepository;
        BlogLikes = blogLikeRepository;
        BlogComments = blogCommentRepository;
        RefreshTokens = refreshTokens;
        Languages = languages;
        Localizations = localizations;
        LocalizationLanguageDetails = localizationLanguageDetails;
    }


    #region Sync Methods
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void BeginTransaction()
    {
        if (_transaction != null) 
            throw new InvalidOperationException("Transaction already started for begin transaction.");

        _transaction = _context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        if (_transaction == null) 
            throw new InvalidOperationException("Transaction has not been started for commit transaction.");

        _transaction.Commit();

        _transaction.Dispose();
        _transaction = null;
    }

    public void RollbackTransaction()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
    }
    #endregion


    #region Async Methods
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started for begin transaction.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) 
            throw new InvalidOperationException("Transaction has not been started for commit.");

        await _transaction.CommitAsync(cancellationToken);

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    #endregion


    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }

        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        await _context.DisposeAsync();
    }
}