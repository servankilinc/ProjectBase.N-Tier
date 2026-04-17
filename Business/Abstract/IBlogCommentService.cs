using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.BlogComment.Commands;
using Model.Dtos.BlogComment.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogCommentService
{
    #region Get
    Task<Result<BlogComment>> GetAsync(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default);
    Task<Result<BlogComment>> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<BlogCommentBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<BlogCommentDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Get List
    Task<Result<ICollection<BlogComment>>> GetListAsync(Expression<Func<BlogComment, bool>>? where = null, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogComment>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogCommentBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<BlogCommentDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    #endregion

    #region SelectList
    Task<Result<SelectList>> SelectListAsync(Expression<Func<BlogComment, bool>>? where = null, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<Result> CreateAsync(BlogCommentCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<Result<BlogCommentUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(BlogCommentUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Pagination
    Task<Result<PaginationResponse<BlogCommentReportDto>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable
    Task<Result<DatatableResponseClientSide<BlogCommentReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    Task<Result<DatatableResponseServerSide<BlogCommentReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    #endregion
}
