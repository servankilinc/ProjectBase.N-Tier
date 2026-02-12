using AutoMapper;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Core.Utils.Validation;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Business.ServiceBase;

public abstract class ServiceBase<TEntity, TRepository>
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>, IRepositoryAsync<TEntity>
{
    protected readonly TRepository _repository;
    protected readonly IValidationService _validationService;
    protected readonly IMapper _mapper;
    public ServiceBase(TRepository repository, IValidationService validationService, IMapper mapper)
    {
        _repository = repository;
        _validationService = validationService;
        _mapper = mapper;
    }


    // ############################# Sync Methods #############################
    #region Add
    protected virtual Result<TEntity> Add(TEntity entity)
    {
        TEntity insertedEntity = _repository.AddAndSave(entity);
        return Result<TEntity>.Success(insertedEntity);
    }

    protected virtual Result<TDtoResponse> Add<TDtoResponse>(TEntity entity) where TDtoResponse : IDto
    {
        TEntity insertedEntity = _repository.AddAndSave(entity);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(insertedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }

    protected virtual Result<TEntity> Add<TDtoRequest>(TDtoRequest insertModel) where TDtoRequest : IDto
    {
        ValidatorResult validationResult = _validationService.Validate(insertModel);
        if (!validationResult.IsValid)
            return Result<TEntity>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity entityToInsert = _mapper.Map<TEntity>(insertModel);
        TEntity insertedEntity = _repository.AddAndSave(entityToInsert);
        return Result<TEntity>.Success(insertedEntity);
    }

    protected virtual Result<TDtoResponse> Add<TDtoRequest, TDtoResponse>(TDtoRequest insertModel) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        ValidatorResult validationResult = _validationService.Validate(insertModel);
        if (!validationResult.IsValid)
            return Result<TDtoResponse>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity entityToInsert = _mapper.Map<TEntity>(insertModel);
        TEntity insertedEntity = _repository.AddAndSave(entityToInsert);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(insertedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }
    #endregion

    #region AddList
    protected virtual Result<ICollection<TEntity>> AddList(IEnumerable<TEntity> entityList)
    {
        ICollection<TEntity> insertedEntityList = _repository.AddAndSave(entityList);
        return Result<ICollection<TEntity>>.Success(insertedEntityList);
    }

    protected virtual Result<ICollection<TDtoResponse>> AddList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto
    {
        ICollection<TEntity> insertedEntityList = _repository.AddAndSave(entityList);
        ICollection<TDtoResponse> responseModelList = _mapper.Map<ICollection<TDtoResponse>>(insertedEntityList);
        return Result<ICollection<TDtoResponse>>.Success(responseModelList);
    }

    protected virtual Result<ICollection<TEntity>> AddList<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto
    {
        ValidatorResult validationResult = _validationService.Validate(insertModelList);
        if (!validationResult.IsValid)
            return Result<ICollection<TEntity>>.Validation(validationResult.Failures, description: $"Validations failed for {nameof(TDtoRequest)}");

        IEnumerable<TEntity> mappedEntityList = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        ICollection<TEntity> insertedEntityList = _repository.AddAndSave(mappedEntityList);
        return Result<ICollection<TEntity>>.Success(insertedEntityList);
    }

    protected virtual Result<ICollection<TDtoResponse>> AddList<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        ValidatorResult validationResult = _validationService.Validate(insertModelList);
        if (!validationResult.IsValid)
            return Result<ICollection<TDtoResponse>>.Validation(validationResult.Failures, description: $"Validations failed for {nameof(TDtoRequest)}");

        IEnumerable<TEntity> entityListToInsert = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        ICollection<TEntity> insertedEntityList = _repository.AddAndSave(entityListToInsert);
        ICollection<TDtoResponse> responseModelList = _mapper.Map<ICollection<TDtoResponse>>(insertedEntityList);
        return Result<ICollection<TDtoResponse>>.Success(responseModelList);
    }
    #endregion

    #region Update
    protected virtual Result<TEntity> Update(TEntity entity)
    {
        TEntity updatedEntity = _repository.UpdateAndSave(entity);
        return Result<TEntity>.Success(updatedEntity);
    }

    protected virtual Result<TDtoResponse> Update<TDtoResponse>(TEntity entity) where TDtoResponse : IDto
    {
        TEntity updatedEntity = _repository.UpdateAndSave(entity);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(updatedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }

    protected virtual Result<TEntity> Update<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto
    {
        ValidatorResult validationResult = _validationService.Validate(updateModel);
        if (!validationResult.IsValid)
            return Result<TEntity>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity? entity = _repository.Get(where: where);
        if (entity == null)
            return Result<TEntity>.NotFound(description: $"The entity({nameof(TEntity)}) was not found to update.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        TEntity updatedEntity = _repository.UpdateAndSave(entityToUpdate);
        return Result<TEntity>.Success(updatedEntity);
    }

    protected virtual Result<TDtoResponse> Update<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        ValidatorResult validationResult = _validationService.Validate(updateModel);
        if (!validationResult.IsValid)
            return Result<TDtoResponse>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity? entity = _repository.Get(where: where);
        if (entity == null)
            return Result<TDtoResponse>.NotFound(description: $"The entity({nameof(TEntity)}) was not found to update.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        TEntity updatedEntity = _repository.UpdateAndSave(entityToUpdate);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(updatedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }
    #endregion

    #region UpdateList
    protected virtual Result<ICollection<TEntity>> UpdateList(IEnumerable<TEntity> entityList)
    {
        ICollection<TEntity> updatedEntityList = _repository.UpdateAndSave(entityList);
        return Result<ICollection<TEntity>>.Success(updatedEntityList);
    }

    protected virtual Result<ICollection<TDtoResponse>> UpdateList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto
    {
        ICollection<TEntity> updatedEntityList = _repository.UpdateAndSave(entityList);
        ICollection<TDtoResponse> responseModelList = _mapper.Map<ICollection<TDtoResponse>>(updatedEntityList);
        return Result<ICollection<TDtoResponse>>.Success(responseModelList);
    }
    #endregion

    #region Delete
    protected virtual Result Delete(TEntity entity)
    {
        _repository.DeleteAndSave(entity);
        return Result.Success();
    }

    protected virtual Result Delete(IEnumerable<TEntity> entityList)
    {
        _repository.DeleteAndSave(entityList);
        return Result.Success();
    }

    protected virtual Result Delete(Expression<Func<TEntity, bool>> where)
    {
        _repository.DeleteAndSave(where);
        return Result.Success();
    }

    protected virtual Result UndoDelete(Expression<Func<TEntity, bool>> where)
    {
        TEntity? originalEntity = _repository.Get(where: where, ignoreFilters: true);

        if (originalEntity == null)
            return Result.NotFound(description: $"The entity({nameof(TEntity)}) was not found to undo deletion.");

        if (originalEntity is not ISoftDeletableEntity softEntity)
            return Result.Failure(description: "The entity must implement ISoftDeletableEntity for undo deletion.");

        softEntity.IsDeleted = false;
        softEntity.DeletedBy = null;
        softEntity.DeletedDateUtc = null;

        _repository.UpdateAndSave(originalEntity);

        return Result.Success();
    }
    #endregion

    #region IsExist & Count
    protected virtual Result<bool> IsExist(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false)
    {
        bool isExist = _repository.IsExist(filter, where, ignoreFilters);
        return Result<bool>.Success(isExist);
    }

    protected virtual Result<int> Count(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false)
    {
        int count = _repository.Count(filter, where, ignoreFilters);
        return Result<int>.Success(count);
    }
    #endregion

    #region Get
    protected virtual Result<TEntity> Get(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true)
    {
        TEntity? entity = _repository.Get(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking
        );

        if (entity == null)
            return Result<TEntity>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<TEntity>.Success(entity);
    }

    protected virtual Result<TDtoResponse> Get<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = _repository.Get(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        if (responseModel == null)
            return Result<TDtoResponse>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<TDtoResponse>.Success(responseModel);
    }

    protected virtual Result<object> Get(
        Expression<Func<TEntity, object>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        object? responseModel = _repository.Get(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        if (responseModel == null)
            return Result<object>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<object>.Success(responseModel);
    }

    protected virtual Result<TDtoResponse> Get<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = _repository.Get<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        if (responseModel == null)
            return Result<TDtoResponse>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<TDtoResponse>.Success(responseModel);
    }
    #endregion

    #region GetList
    protected virtual Result<ICollection<TEntity>> GetList(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true)
    {
        ICollection<TEntity>? entities = _repository.GetAll(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking
        );

        if (entities == null)
            return Result<ICollection<TEntity>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<TEntity>>.Success(entities);
    }

    protected virtual Result<ICollection<TDtoResponse>> GetList<TDtoResponse>(
       Expression<Func<TEntity, TDtoResponse>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
       bool ignoreFilters = false) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = _repository.GetAll(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        if (responseModel == null)
            return Result<ICollection<TDtoResponse>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<TDtoResponse>>.Success(responseModel);
    }

    protected virtual Result<ICollection<object>> GetList(
       Expression<Func<TEntity, object>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
       bool ignoreFilters = false)
    {
        ICollection<object>? responseModel = _repository.GetAll(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        if (responseModel == null)
            return Result<ICollection<object>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<object>>.Success(responseModel);
    }

    protected virtual Result<ICollection<TDtoResponse>> GetList<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = _repository.GetAll<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        if (responseModel == null)
            return Result<ICollection<TDtoResponse>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<TDtoResponse>>.Success(responseModel);
    }
    #endregion

    #region Datatable Server-Side
    protected virtual Result<DatatableResponseServerSide<TEntity>> DatatableServerSide(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        DatatableResponseServerSide<TEntity> data = _repository.DatatableServerSide(
            datatableRequest: datatableRequest,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<DatatableResponseServerSide<TEntity>>.Success(data);
    }

    protected virtual Result<DatatableResponseServerSide<TDtoResponse>> DatatableServerSide<TDtoResponse>(
       DynamicDatatableRequest datatableRequest,
       Expression<Func<TEntity, TDtoResponse>> select,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
       bool ignoreFilters = false) where TDtoResponse : IDto
    {
        DatatableResponseServerSide<TDtoResponse> data = _repository.DatatableServerSide(
            datatableRequest: datatableRequest,
            select: select,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<DatatableResponseServerSide<TDtoResponse>>.Success(data);
    }

    protected virtual Result<DatatableResponseServerSide<TDtoResponse>> DatatableServerSide<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        DatatableResponseServerSide<TDtoResponse> data = _repository.DatatableServerSide<TDtoResponse>(
            datatableRequest: datatableRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<DatatableResponseServerSide<TDtoResponse>>.Success(data);
    }
    #endregion

    #region Datatable Client-Side
    protected virtual Result<DatatableResponseClientSide<TEntity>> DatatableClientSide(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        DatatableResponseClientSide<TEntity> data = _repository.DatatableClientSide(
            datatableRequest: datatableRequest,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<DatatableResponseClientSide<TEntity>>.Success(data);
    }

    protected virtual Result<DatatableResponseClientSide<TDtoResponse>> DatatableClientSide<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        DatatableResponseClientSide<TDtoResponse> data = _repository.DatatableClientSide(
            datatableRequest: datatableRequest,
            select: select,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<DatatableResponseClientSide<TDtoResponse>>.Success(data);
    }

    protected virtual Result<DatatableResponseClientSide<TDtoResponse>> DatatableClientSide<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        DatatableResponseClientSide<TDtoResponse> data = _repository.DatatableClientSide<TDtoResponse>(
            datatableRequest: datatableRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<DatatableResponseClientSide<TDtoResponse>>.Success(data);
    }
    #endregion

    #region Pagination
    protected virtual Result<PaginationResponse<TEntity>> Pagination(
        DynamicPaginationRequest paginationRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        PaginationResponse<TEntity> data = _repository.Pagination(
            paginationRequest: paginationRequest,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<PaginationResponse<TEntity>>.Success(data);
    }

    protected virtual Result<PaginationResponse<TDtoResponse>> Pagination<TDtoResponse>(
        DynamicPaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        PaginationResponse<TDtoResponse> data = _repository.Pagination(
            paginationRequest: paginationRequest,
            select: select,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<PaginationResponse<TDtoResponse>>.Success(data);
    }

    protected virtual Result<PaginationResponse<TDtoResponse>> Pagination<TDtoResponse>(
        DynamicPaginationRequest paginationRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        PaginationResponse<TDtoResponse> data = _repository.Pagination<TDtoResponse>(
            paginationRequest: paginationRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters
        );

        return Result<PaginationResponse<TDtoResponse>>.Success(data);
    }
    #endregion

    // ############################# Async Methods #############################
    #region Add
    protected virtual async Task<Result<TEntity>> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        TEntity insertedEntity = await _repository.AddAndSaveAsync(entity, cancellationToken);
        return Result<TEntity>.Success(insertedEntity);
    }

    protected virtual async Task<Result<TDtoResponse>> CreateAsync<TDtoResponse>(TEntity entity, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TEntity insertedEntity = await _repository.AddAndSaveAsync(entity, cancellationToken);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(insertedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }

    protected virtual async Task<Result<TEntity>> CreateAsync<TDtoRequest>(TDtoRequest insertModel, CancellationToken cancellationToken = default) where TDtoRequest : IDto
    {
        ValidatorResult validationResult = await _validationService.ValidateAsync(insertModel, cancellationToken);
        if (!validationResult.IsValid)
            return Result<TEntity>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity entityToInsert = _mapper.Map<TEntity>(insertModel);
        TEntity insertedEntity = await _repository.AddAndSaveAsync(entityToInsert, cancellationToken);
        return Result<TEntity>.Success(insertedEntity);
    }

    protected virtual async Task<Result<TDtoResponse>> CreateAsync<TDtoRequest, TDtoResponse>(TDtoRequest insertModel, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        ValidatorResult validationResult = await _validationService.ValidateAsync(insertModel, cancellationToken);
        if (!validationResult.IsValid)
            return Result<TDtoResponse>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity entityToInsert = _mapper.Map<TEntity>(insertModel);
        TEntity insertedEntity = await _repository.AddAndSaveAsync(entityToInsert, cancellationToken);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(insertedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }
    #endregion

    #region AddList
    protected virtual async Task<Result<ICollection<TEntity>>> AddListAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default)
    {
        ICollection<TEntity> insertedEntityList = await _repository.AddAndSaveAsync(entityList, cancellationToken);
        return Result<ICollection<TEntity>>.Success(insertedEntityList);
    }

    protected virtual async Task<Result<ICollection<TDtoResponse>>> AddListAsync<TDtoResponse>(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        ICollection<TEntity> insertedEntityList = await _repository.AddAndSaveAsync(entityList, cancellationToken);
        ICollection<TDtoResponse> responseModelList = _mapper.Map<ICollection<TDtoResponse>>(insertedEntityList);
        return Result<ICollection<TDtoResponse>>.Success(responseModelList);
    }

    protected virtual async Task<Result<ICollection<TEntity>>> AddListAsync<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList, CancellationToken cancellationToken = default) where TDtoRequest : IDto
    {
        ValidatorResult validationResult = await _validationService.ValidateAsync(insertModelList, cancellationToken);
        if (!validationResult.IsValid)
            return Result<ICollection<TEntity>>.Validation(validationResult.Failures, description: $"Validations failed for {nameof(TDtoRequest)}");

        IEnumerable<TEntity> mappedEntityList = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        ICollection<TEntity> insertedEntityList = await _repository.AddAndSaveAsync(mappedEntityList, cancellationToken);
        return Result<ICollection<TEntity>>.Success(insertedEntityList);
    }

    protected virtual async Task<Result<ICollection<TDtoResponse>>> AddListAsync<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        ValidatorResult validationResult = await _validationService.ValidateAsync(insertModelList, cancellationToken);
        if (!validationResult.IsValid)
            return Result<ICollection<TDtoResponse>>.Validation(validationResult.Failures, description: $"Validations failed for {nameof(TDtoRequest)}");

        IEnumerable<TEntity> entityListToInsert = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        ICollection<TEntity> insertedEntityList = await _repository.AddAndSaveAsync(entityListToInsert, cancellationToken);
        ICollection<TDtoResponse> responseModelList = _mapper.Map<ICollection<TDtoResponse>>(insertedEntityList);
        return Result<ICollection<TDtoResponse>>.Success(responseModelList);
    }
    #endregion

    #region Update
    protected virtual async Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        TEntity updatedEntity = await _repository.UpdateAndSaveAsync(entity, cancellationToken);
        return Result<TEntity>.Success(updatedEntity);
    }

    protected virtual async Task<Result<TDtoResponse>> UpdateAsync<TDtoResponse>(TEntity entity, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TEntity updatedEntity = await _repository.UpdateAndSaveAsync(entity, cancellationToken);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(updatedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }

    protected virtual async Task<Result<TEntity>> UpdateAsync<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) where TDtoRequest : IDto
    {
        ValidatorResult validationResult = await _validationService.ValidateAsync(updateModel, cancellationToken);
        if (!validationResult.IsValid)
            return Result<TEntity>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity? entity = await _repository.GetAsync(where: where, cancellationToken: cancellationToken);
        if (entity == null)
            return Result<TEntity>.NotFound(description: $"The entity({nameof(TEntity)}) was not found to update.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        TEntity updatedEntity = await _repository.UpdateAndSaveAsync(entityToUpdate, cancellationToken);
        return Result<TEntity>.Success(updatedEntity);
    }

    protected virtual async Task<Result<TDtoResponse>> UpdateAsync<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        ValidatorResult validationResult = await _validationService.ValidateAsync(updateModel, cancellationToken);
        if (!validationResult.IsValid)
            return Result<TDtoResponse>.Validation(validationResult.Failures, description: $"Validation failed for {nameof(TDtoRequest)}");

        TEntity? entity = await _repository.GetAsync(where: where, cancellationToken: cancellationToken);
        if (entity == null)
            return Result<TDtoResponse>.NotFound(description: $"The entity({nameof(TEntity)}) was not found to update.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        TEntity updatedEntity = await _repository.UpdateAndSaveAsync(entityToUpdate, cancellationToken);
        TDtoResponse responseModel = _mapper.Map<TDtoResponse>(updatedEntity);
        return Result<TDtoResponse>.Success(responseModel);
    }
    #endregion

    #region UpdateList
    protected virtual async Task<Result<ICollection<TEntity>>> UpdateListAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default)
    {
        ICollection<TEntity> updatedEntityList = await _repository.UpdateAndSaveAsync(entityList, cancellationToken);
        return Result<ICollection<TEntity>>.Success(updatedEntityList);
    }

    protected virtual async Task<Result<ICollection<TDtoResponse>>> UpdateListAsync<TDtoResponse>(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        ICollection<TEntity> updatedEntityList = await _repository.UpdateAndSaveAsync(entityList, cancellationToken);
        ICollection<TDtoResponse> responseModelList = _mapper.Map<ICollection<TDtoResponse>>(updatedEntityList);
        return Result<ICollection<TDtoResponse>>.Success(responseModelList);
    }
    #endregion

    #region Delete
    protected virtual async Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAndSaveAsync(entity, cancellationToken);
        return Result.Success();
    }

    protected virtual async Task<Result> DeleteAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAndSaveAsync(entityList, cancellationToken);
        return Result.Success();
    }

    protected virtual async Task<Result> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAndSaveAsync(where, cancellationToken);
        return Result.Success();
    }

    protected virtual async Task<Result> UndoDeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
    {
        TEntity? originalEntity = await _repository.GetAsync(where: where, ignoreFilters: true, cancellationToken: cancellationToken);

        if (originalEntity == null)
            return Result.NotFound(description: $"The entity({nameof(TEntity)}) was not found to undo deletion.");

        if (originalEntity is not ISoftDeletableEntity softEntity)
            return Result.Failure(description: "The entity must implement ISoftDeletableEntity for undo deletion.");

        softEntity.IsDeleted = false;
        softEntity.DeletedBy = null;
        softEntity.DeletedDateUtc = null;

        await _repository.UpdateAndSaveAsync(originalEntity, cancellationToken);

        return Result.Success();
    }
    #endregion

    #region IsExist & Count
    protected virtual async Task<Result<bool>> IsExistAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false, CancellationToken cancellationToken = default)
    {
        bool isExist = await _repository.IsExistAsync(filter, where, ignoreFilters, cancellationToken: cancellationToken);
        return Result<bool>.Success(isExist);
    }

    protected virtual async Task<Result<int>> CountAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false, CancellationToken cancellationToken = default)
    {
        int count = await _repository.CountAsync(filter, where, ignoreFilters, cancellationToken: cancellationToken);
        return Result<int>.Success(count);
    }
    #endregion

    #region Get
    protected virtual async Task<Result<TEntity>> GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _repository.GetAsync(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken
        );

        if (entity == null)
            return Result<TEntity>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<TEntity>.Success(entity);
    }

    protected virtual async Task<Result<TDtoResponse>> GetAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = await _repository.GetAsync(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        if (responseModel == null)
            return Result<TDtoResponse>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<TDtoResponse>.Success(responseModel);
    }

    protected virtual async Task<Result<object>> GetAsync(
        Expression<Func<TEntity, object>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        object? responseModel = await _repository.GetAsync(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        if (responseModel == null)
            return Result<object>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<object>.Success(responseModel);
    }

    protected virtual async Task<Result<TDtoResponse>> GetAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = await _repository.GetAsync<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        if (responseModel == null)
            return Result<TDtoResponse>.NotFound(description: $"The entity({nameof(TEntity)}) was not found.");

        return Result<TDtoResponse>.Success(responseModel);
    }
    #endregion

    #region GetList
    protected virtual async Task<Result<ICollection<TEntity>>> GetListAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        ICollection<TEntity>? entities = await _repository.GetAllAsync(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken
        );

        if (entities == null)
            return Result<ICollection<TEntity>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<TEntity>>.Success(entities);
    }

    protected virtual async Task<Result<ICollection<TDtoResponse>>> GetListAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = await _repository.GetAllAsync(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        if (responseModel == null)
            return Result<ICollection<TDtoResponse>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<TDtoResponse>>.Success(responseModel);
    }

    protected virtual async Task<Result<ICollection<object>>> GetListAsync(
        Expression<Func<TEntity, object>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        ICollection<object>? responseModel = await _repository.GetAllAsync(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        if (responseModel == null)
            return Result<ICollection<object>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<object>>.Success(responseModel);
    }

    protected virtual async Task<Result<ICollection<TDtoResponse>>> GetListAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = await _repository.GetAllAsync<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        if (responseModel == null)
            return Result<ICollection<TDtoResponse>>.NotFound(description: $"The entities({nameof(TEntity)}) was not found.");

        return Result<ICollection<TDtoResponse>>.Success(responseModel);
    }
    #endregion

    #region Datatable Server-Side
    protected virtual async Task<Result<DatatableResponseServerSide<TEntity>>> DatatableServerSideAsync(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = true,
        CancellationToken cancellationToken = default)
    {
        DatatableResponseServerSide<TEntity> data = await _repository.DatatableServerSideAsync(
            datatableRequest: datatableRequest,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<DatatableResponseServerSide<TEntity>>.Success(data);
    }

    protected virtual async Task<Result<DatatableResponseServerSide<TDtoResponse>>> DatatableServerSideAsync<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = true,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        DatatableResponseServerSide<TDtoResponse> data = await _repository.DatatableServerSideAsync<TDtoResponse>(
            datatableRequest: datatableRequest,
            select: select,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<DatatableResponseServerSide<TDtoResponse>>.Success(data);
    }

    protected virtual async Task<Result<DatatableResponseServerSide<TDtoResponse>>> DatatableServerSideAsync<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = true,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        DatatableResponseServerSide<TDtoResponse> data = await _repository.DatatableServerSideAsync<TDtoResponse>(
            datatableRequest: datatableRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<DatatableResponseServerSide<TDtoResponse>>.Success(data);
    }
    #endregion

    #region Datatable Client-Side
    protected virtual async Task<Result<DatatableResponseClientSide<TEntity>>> DatatableClientSideAsync(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = true,
        CancellationToken cancellationToken = default)
    {
        DatatableResponseClientSide<TEntity> data = await _repository.DatatableClientSideAsync(
            datatableRequest: datatableRequest,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<DatatableResponseClientSide<TEntity>>.Success(data);
    }

    protected virtual async Task<Result<DatatableResponseClientSide<TDtoResponse>>> DatatableClientSideAsync<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = true,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        DatatableResponseClientSide<TDtoResponse> data = await _repository.DatatableClientSideAsync<TDtoResponse>(
            datatableRequest: datatableRequest,
            select: select,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<DatatableResponseClientSide<TDtoResponse>>.Success(data);
    }

    protected virtual async Task<Result<DatatableResponseClientSide<TDtoResponse>>> DatatableClientSideAsync<TDtoResponse>(
        DynamicDatatableRequest datatableRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = true,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        DatatableResponseClientSide<TDtoResponse> data = await _repository.DatatableClientSideAsync<TDtoResponse>(
            datatableRequest: datatableRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<DatatableResponseClientSide<TDtoResponse>>.Success(data);
    }
    #endregion

    #region Pagination
    protected virtual async Task<Result<PaginationResponse<TEntity>>> PaginationAsync(
        DynamicPaginationRequest paginationRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        PaginationResponse<TEntity> data = await _repository.PaginationAsync(
            paginationRequest: paginationRequest,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<PaginationResponse<TEntity>>.Success(data);
    }

    protected virtual async Task<Result<PaginationResponse<TDtoResponse>>> PaginationAsync<TDtoResponse>(
        DynamicPaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        PaginationResponse<TDtoResponse> data = await _repository.PaginationAsync<TDtoResponse>(
            paginationRequest: paginationRequest,
            select: select,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<PaginationResponse<TDtoResponse>>.Success(data);
    }

    protected virtual async Task<Result<PaginationResponse<TDtoResponse>>> PaginationAsync<TDtoResponse>(
        DynamicPaginationRequest paginationRequest,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        PaginationResponse<TDtoResponse> data = await _repository.PaginationAsync<TDtoResponse>(
            paginationRequest: paginationRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken
        );

        return Result<PaginationResponse<TDtoResponse>>.Success(data);
    }
    #endregion
}
