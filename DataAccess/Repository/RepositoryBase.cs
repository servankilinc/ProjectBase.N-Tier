using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Model.Entities;
using System.Linq.Expressions;

namespace DataAccess.Repository;

public class RepositoryBase<TEntity, TContext> : IRepository<TEntity>, IRepositoryAsync<TEntity>
    where TEntity : class, IEntity
    where TContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    // DbContext 
    // IdentityDbContext<User, IdentityRole<Guid>, Guid> && IdentityDbContext<User, Role<Guid>, Guid>
{
    protected TContext _context { get; set; }
    public RepositoryBase(TContext context) => _context = context;


    // ############################# Sync Methods #############################
    #region Add
    public TEntity Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        return entity;
    }

    public List<TEntity> Add(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        return entities.ToList();
    }

    public TEntity AddAndSave(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public List<TEntity> AddAndSave(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        _context.SaveChanges();
        return entities.ToList();
    }
    #endregion

    #region Update
    public TEntity Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public List<TEntity> Update(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        return entities.ToList();
    }

    public TEntity UpdateAndSave(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
        return entity;
    }

    public List<TEntity> UpdateAndSave(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        _context.SaveChanges();
        return entities.ToList();
    }
    #endregion

    #region Delete
    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public void Delete(Expression<Func<TEntity, bool>> where)
    {
        var entitiesToDelete = _context.Set<TEntity>().Where(where);
        _context.Set<TEntity>().RemoveRange(entitiesToDelete);
    }

    public void DeleteAndSave(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        _context.SaveChanges();
    }

    public void DeleteAndSave(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        _context.SaveChanges();
    }

    public void DeleteAndSave(Expression<Func<TEntity, bool>> where)
    {
        var entitiesToDelete = _context.Set<TEntity>().Where(where);
        _context.Set<TEntity>().RemoveRange(entitiesToDelete);
        _context.SaveChanges();
    }
    #endregion

    #region IsExist & Count
    public bool IsExist(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (ignoreFilters) query = query.IgnoreQueryFilters();

        return query.Any();
    }

    public int Count(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (ignoreFilters) query = query.IgnoreQueryFilters();

        return query.Count();
    }
    #endregion

    #region Get
    public TEntity? Get(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return query.FirstOrDefault();
    }

    public TResult? Get<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        if (typeof(ILocalizableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var entity = query.FirstOrDefault();

            var selector = select.Compile();
            if (entity == null) return default;
            return selector(entity);
        }
        else
        {
            return  query.Select(select).FirstOrDefault();
        }
    }

    public TResult? Get<TResult>(
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false
    )
    {
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return query.ProjectTo<TResult>(configurationProvider).FirstOrDefault();
    }

    public TResult? Get<TResult>(
        IMapper mapper,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false
    )
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        var entity = query.FirstOrDefault();

        return mapper.Map<TResult>(entity);
    }
    #endregion

    #region GetAll
    public ICollection<TEntity>? GetAll(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return query.ToList();
    }

    public ICollection<TResult>? GetAll<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        if (typeof(ILocalizableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var entities = query.ToList();
            var selector = select.Compile();
            return entities.Select(selector).ToList();
        }
        else
        {
            return query.Select(select).ToList();
        }
    }

    public ICollection<TResult>? GetAll<TResult>(
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false)
    {
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return query.ProjectTo<TResult>(configurationProvider).ToList();
    }

    public ICollection<TResult>? GetAll<TResult>(
        IMapper mapper,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        var entities = query.ToList();

        return mapper.Map<ICollection<TResult>>(entities);
    }
    #endregion

    #region Datatable Server-Side
    public DatatableResponseServerSide<TEntity> DatatableServerSide(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ToDatatableServerSide(datatableRequest);
    }

    public DatatableResponseServerSide<TResult> DatatableServerSide<TResult>(
        DatatableRequest datatableRequest,
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.Select(select).ToDatatableServerSide(datatableRequest);
    }

    public DatatableResponseServerSide<TResult> DatatableServerSide<TResult>(
        DatatableRequest datatableRequest,
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ProjectTo<TResult>(configurationProvider).ToDatatableServerSide(datatableRequest);
    }
    #endregion

    #region Datatable Client-Side
    public DatatableResponseClientSide<TEntity> DatatableClientSide(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ToDatatableClientSide();
    }

    public DatatableResponseClientSide<TResult> DatatableClientSide<TResult>(
      Expression<Func<TEntity, TResult>> select,
      Filter? filter = null,
      IEnumerable<Sort>? sorts = null,
      Expression<Func<TEntity, bool>>? where = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
      bool ignoreFilters = false)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.Select(select).ToDatatableClientSide();
    }

    public DatatableResponseClientSide<TResult> DatatableClientSide<TResult>(
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ProjectTo<TResult>(configurationProvider).ToDatatableClientSide();
    }
    #endregion

    #region Pagination
    public PaginationResponse<TEntity> Pagination(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ToPaginate(paginationRequest);
    }

    public PaginationResponse<TResult> Pagination<TResult>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.Select(select).ToPaginate(paginationRequest);
    }

    public PaginationResponse<TResult> Pagination<TResult>(
        PaginationRequest paginationRequest,
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ProjectTo<TResult>(configurationProvider).ToPaginate(paginationRequest);
    }
    #endregion


    // ############################# Async Methods #############################
    #region Add
    public async Task<TEntity> AddAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<List<TEntity>> AddAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().AddRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
        return entities.ToList();
    }
    #endregion

    #region Update
    public async Task<TEntity> UpdateAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<List<TEntity>> UpdateAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
        return entities.ToList();
    }
    #endregion

    #region Delete
    public async Task DeleteAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAndSaveAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
    {
        var entitiesToDelete = _context.Set<TEntity>().Where(where);
        _context.Set<TEntity>().RemoveRange(entitiesToDelete);
        await _context.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region IsExist & Count
    public async Task<bool> IsExistAsync(
        Filter? filter = null,
        Expression<Func<TEntity, bool>>? where = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (ignoreFilters) query = query.IgnoreQueryFilters();

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Filter? filter = null,
        Expression<Func<TEntity, bool>>? where = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (ignoreFilters) query = query.IgnoreQueryFilters();

        return await query.CountAsync(cancellationToken);
    }
    #endregion

    #region Get
    public async Task<TEntity?> GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        if (typeof(ILocalizableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var entity = await query.FirstOrDefaultAsync(cancellationToken);

            var selector = select.Compile();
            if (entity == null) return default;
            return selector(entity);
        }
        else
        {
            return await query.Select(select).FirstOrDefaultAsync(cancellationToken);
        }
    }

    public async Task<TResult?> GetAsync<TResult>(
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return await query.ProjectTo<TResult>(configurationProvider).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetAsync<TResult>(
        IMapper mapper,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        var entity =  await query.FirstOrDefaultAsync(cancellationToken);
        return mapper.Map<TResult>(entity);
    }
    #endregion

    #region GetAll
    public async Task<ICollection<TEntity>?> GetAllAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<ICollection<TResult>?> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        if (typeof(ILocalizableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var entities = await query.ToListAsync();
            var selector = select.Compile();
            return entities.Select(selector).ToList();
        }
        else
        {
            return await query.Select(select).ToListAsync();
        }
    }

    public async Task<ICollection<TResult>?> GetAllAsync<TResult>(
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();

        return await query.ProjectTo<TResult>(configurationProvider).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<TResult>?> GetAllAsync<TResult>(
        IMapper mapper,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        if (!tracking) query = query.AsNoTracking();
         
        var entities = await query.ToListAsync(cancellationToken);

        return mapper.Map<ICollection<TResult>>(entities);
    }
    #endregion

    #region Datatable Server-Side
    public async Task<DatatableResponseServerSide<TEntity>> DatatableServerSideAsync(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ToDatatableServerSideAsync(datatableRequest, cancellationToken);
    }

    public async Task<DatatableResponseServerSide<TResult>> DatatableServerSideAsync<TResult>(
        DatatableRequest datatableRequest,
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.Select(select).ToDatatableServerSideAsync(datatableRequest, cancellationToken);
    }

    public async Task<DatatableResponseServerSide<TResult>> DatatableServerSideAsync<TResult>(
        DatatableRequest datatableRequest,
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ProjectTo<TResult>(configurationProvider).ToDatatableServerSideAsync(datatableRequest, cancellationToken);
    }
    #endregion

    #region Datatable Client-Side
    public async Task<DatatableResponseClientSide<TEntity>> DatatableClientSideAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ToDatatableClientSideAsync(cancellationToken);
    }

    public async Task<DatatableResponseClientSide<TResult>> DatatableClientSideAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.Select(select).ToDatatableClientSideAsync(cancellationToken);
    }

    public async Task<DatatableResponseClientSide<TResult>> DatatableClientSideAsync<TResult>(
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ProjectTo<TResult>(configurationProvider).ToDatatableClientSideAsync(cancellationToken);
    }
    #endregion

    #region Pagination
    public async Task<PaginationResponse<TEntity>> PaginationAsync(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ToPaginateAsync(paginationRequest, cancellationToken);
    }

    public async Task<PaginationResponse<TResult>> PaginationAsync<TResult>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.Select(select).ToPaginateAsync(paginationRequest, cancellationToken);
    }

    public async Task<PaginationResponse<TResult>> PaginationAsync<TResult>(
        PaginationRequest paginationRequest,
        IConfigurationProvider configurationProvider,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));
        if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (ignoreFilters) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ProjectTo<TResult>(configurationProvider).ToPaginateAsync(paginationRequest, cancellationToken);
    }
    #endregion
}
