using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Model.Dtos.Blog.Queries;
using Model.Dtos.BlogLike.Commands;
using Model.Dtos.BlogLike.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogLikeService
{
    #region Get
    Task<Result<BlogLike>> GetAsync(Expression<Func<BlogLike, bool>> where, CancellationToken cancellationToken = default);
    Task<Result<BlogLike>> GetAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    Task<Result<BlogLikeResponseDto>> GetBasicAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    Task<Result<BlogLikeListResponseDto>> GetDetailAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    #endregion

    #region GetList
    Task<Result<ICollection<BlogLike>>> GetListAsync(Expression<Func<BlogLike, bool>>? where = null, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogLike>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogLikeResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogLikeListResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    #endregion

    #region SelectList
    // Multiple Primary Key...
    #endregion

    #region Create
    Task<Result<BlogLikeResponseDto>> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<Result<BlogLikeResponseDto>> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task<Result> DeleteAsync(BlogLikeDeleteDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Pagination
    Task<Result<PaginationResponse<BlogLike>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable
    Task<Result<DatatableResponseClientSide<BlogLike>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    Task<Result<DatatableResponseServerSide<BlogLike>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    #endregion
}
