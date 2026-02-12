using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Core.Utils.Validation;
using DataAccess.Abstract;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Blog.Queries;
using Model.Dtos.BlogLike.Commands;
using Model.Dtos.BlogLike.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogLikeService : ServiceBase<BlogLike, IBlogLikeRepository>, IBlogLikeService
{
    public BlogLikeService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper) : base(unitOfWork.BlogLikes, validationService, mapper)
    {
    }

    #region Get
    public async Task<Result<BlogLike>> GetAsync(Expression<Func<BlogLike, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogLike>> GetAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogLikeResponseDto>> GetBasicAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogLikeResponseDto>(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogLikeListResponseDto>> GetDetailAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogLikeListResponseDto>(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region GetList 
    public async Task<Result<ICollection<BlogLike>>> GetListAsync(Expression<Func<BlogLike, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogLike>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogLikeResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogLikeResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogLikeListResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogLikeListResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region SelectList
    // Multiple Primary Key...
    #endregion

    #region Create
    public async Task<Result<BlogLikeResponseDto>> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.CreateAsync<BlogLikeCreateDto, BlogLikeResponseDto>(request, cancellationToken);
        return result;
    }
    #endregion

    #region Update
    public async Task<Result<BlogLikeResponseDto>> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default)
    {
        // where: f => f.UserId == request.UserId && f.BlogId == request.BlogId, 
        var result = await base.UpdateAsync<BlogLikeResponseDto>(request, cancellationToken);
        return result;
    }
    #endregion

    #region Delete 
    public async Task<Result> DeleteAsync(BlogLikeDeleteDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.DeleteAsync(where: f => f.BlogId == request.BlogId && f.UserId == request.UserId, cancellationToken);
        return result;
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<BlogLike>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region Datatable Methods
    public async Task<Result<DatatableResponseClientSide<BlogLike>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableClientSideAsync(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<DatatableResponseServerSide<BlogLike>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableServerSideAsync(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion
}
