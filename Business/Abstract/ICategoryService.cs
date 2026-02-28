using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Category.Commands;
using Model.Dtos.Category.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ICategoryService
{
    #region Get
    Task<Result<Category>> GetAsync(Expression<Func<Category, bool>> where, CancellationToken cancellationToken = default);
    Task<Result<Category>> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<CategoryBlogsResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region GetList
    Task<Result<ICollection<Category>>> GetListAsync(Expression<Func<Category, bool>>? where = null, CancellationToken cancellationToken = default);
    Task<Result<ICollection<Category>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<CategoryResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<CategoryBlogsResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    #endregion

    #region SelectList
    Task<Result<SelectList>> SelectListAsync(Expression<Func<Category, bool>>? where = null, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<Result<CategoryResponseDto>> CreateAsync(CategoryCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<Result<CategoryUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponseDto>> UpdateAsync(CategoryUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Pagination
    Task<Result<PaginationResponse<Category>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    Task<Result<PaginationResponse<CategoryReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable
    Task<Result<DatatableResponseClientSide<CategoryReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    Task<Result<DatatableResponseServerSide<CategoryReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    #endregion
}
