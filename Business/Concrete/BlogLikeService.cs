using AutoMapper;
using Business.Abstract;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Core.Utils.Validation;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Blog.Queries;
using Model.Dtos.BlogLike.Commands;
using Model.Dtos.BlogLike.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogLikeService : IBlogLikeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public BlogLikeService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }

    #region Get
    public async Task<Result<BlogLike>> GetAsync(Expression<Func<BlogLike, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogLike>.NotFound();

        return Result<BlogLike>.Success(result);
    }

    public async Task<Result<BlogLike>> GetAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAsync(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogLike>.NotFound();

        return Result<BlogLike>.Success(result);
    }

    public async Task<Result<BlogLikeResponseDto>> GetBasicAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAsync<BlogLikeResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogLikeResponseDto>.NotFound();

        return Result<BlogLikeResponseDto>.Success(result);
    }

    public async Task<Result<BlogLikeListResponseDto>> GetDetailAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAsync<BlogLikeListResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogLikeListResponseDto>.NotFound();

        return Result<BlogLikeListResponseDto>.Success(result);
    }
    #endregion

    #region GetList 
    public async Task<Result<ICollection<BlogLike>>> GetListAsync(Expression<Func<BlogLike, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAllAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogLike>>.NotFound();

        return Result<ICollection<BlogLike>>.Success(result);
    }

    public async Task<Result<ICollection<BlogLike>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAllAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogLike>>.NotFound();

        return Result<ICollection<BlogLike>>.Success(result);
    }

    public async Task<Result<ICollection<BlogLikeResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAllAsync<BlogLikeResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogLikeResponseDto>>.NotFound();

        return Result<ICollection<BlogLikeResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<BlogLikeListResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.GetAllAsync<BlogLikeListResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogLikeListResponseDto>>.NotFound();

        return Result<ICollection<BlogLikeListResponseDto>>.Success(result);
    }
    #endregion

    #region SelectList
    // Multiple Primary Key...
    #endregion

    #region Create
    public async Task<Result> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Validation(validationResult.Failures, description: $"Validation failed for BlogLikeCreateDto");

        await _unitOfWork.BlogLikes.AddAndSaveAsync(_mapper.Map<BlogLike>(request), cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Update
    public async Task<Result> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Validation(validationResult.Failures, description: $"Validation failed for BlogLike");

        var entity = await _unitOfWork.BlogLikes.GetAsync(where: f => f.UserId == request.UserId && f.BlogId == request.BlogId, cancellationToken: cancellationToken);
        if (entity == null)
            return Result.NotFound();

        await _unitOfWork.BlogLikes.UpdateAndSaveAsync(_mapper.Map(request, entity), cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Delete 
    public async Task<Result> DeleteAsync(BlogLikeDeleteDto request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BlogLikes.DeleteAndSaveAsync(where: f => f.BlogId == request.BlogId && f.UserId == request.UserId, cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<BlogLike>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<BlogLike>>.Success(result);
    }
    #endregion

    #region Datatable Methods
    public async Task<Result<DatatableResponseClientSide<BlogLike>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.DatatableClientSideAsync(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseClientSide<BlogLike>>.Success(result);
    }

    public async Task<Result<DatatableResponseServerSide<BlogLike>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogLikes.DatatableServerSideAsync(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseServerSide<BlogLike>>.Success(result);
    }
    #endregion
}
