using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.User.Commands;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserService
{
    #region Get
    Task<Result<User>> GetAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default);
    Task<Result<User>> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<UserBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<UserDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result<UserBlogsResponseDto>> GetUserBlogsResponseDtoAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Get List
    Task<Result<ICollection<User>>> GetListAsync(Expression<Func<User, bool>>? where = null, CancellationToken cancellationToken = default);
    Task<Result<ICollection<User>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<UserBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<UserDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<Result<ICollection<UserBlogsResponseDto>>> GetUserBlogsResponseDtoListAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    #endregion

    #region SelectList
    Task<Result<SelectList>> SelectListAsync(Expression<Func<User, bool>>? where = null, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<Result> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<Result<UserUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Pagination
    Task<Result<PaginationResponse<UserReportDto>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable
    Task<Result<DatatableResponseClientSide<UserReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    Task<Result<DatatableResponseServerSide<UserReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default);
    #endregion
}