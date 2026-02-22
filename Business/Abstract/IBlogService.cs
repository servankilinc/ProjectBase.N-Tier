using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog.Commands;
using Model.Dtos.Blog.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogService
{
    #region Get
    Task<Result<Blog>> GetAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default);
    Task<Result<Blog>> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<BlogBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<BlogDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<BlogLikeListResponseDto>> GetBlogLikeListResponseDtoAsync(Guid id, CancellationToken cancellationToken = default);
    #endregion

    #region GetList
    Task<Result<ICollection<Blog>>> GetListAsync(Expression<Func<Blog, bool>>? where = null, CancellationToken cancellationToken = default);
    Task<Result<ICollection<Blog>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogLikeListResponseDto>>> GetBlogLikeListResponseDtoListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    #endregion

    #region SelectList
    Task<Result<SelectList>> SelectListAsync(Expression<Func<Blog, bool>>? where = null, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<Result<BlogBasicResponseDto>> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<Result<BlogUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<BlogBasicResponseDto>> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result> UndoDeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Pagination
    Task<Result<PaginationResponse<Blog>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    Task<Result<PaginationResponse<BlogReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable
    Task<Result<DatatableResponseClientSide<BlogReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    Task<Result<DatatableResponseServerSide<BlogReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    #endregion
}
