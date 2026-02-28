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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Category.Commands;
using Model.Dtos.User.Commands;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class UserService : ServiceBase<User, IUserRepository>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper) : base(unitOfWork.Users, validationService, mapper)
    {
    }

    #region Get
    public async Task<Result<User>> GetAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<User>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<UserBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<UserBasicResponseDto>(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<UserDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<UserDetailResponseDto>(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<UserBlogsResponseDto>> GetUserBlogsResponseDtoAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<UserBlogsResponseDto>(
            where: f => f.Id == Id,
            include: i => i.Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region Get List
    public async Task<Result<ICollection<User>>> GetListAsync(Expression<Func<User, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<User>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<UserBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<UserBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<UserDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<UserDetailResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<UserBlogsResponseDto>>> GetUserBlogsResponseDtoListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<UserBlogsResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<User, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await base.GetListAsync(
            select: s => new
            {
                s.Id,
                s.Name
            },
            where: where,
            cancellationToken: cancellationToken
        );
        var selectList = new SelectList(list.Data ?? new List<object>(), "Id", "Name");

        return Result<SelectList>.Success(selectList);
    }
    #endregion

    #region Create
    public async Task<Result<UserBasicResponseDto>> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default)
    {
        //var userExist = await _userManager.FindByNameAsync(request.UserName);
        //if (userExist != null)
        //    throw new BusinessException("Kullanıcı adı sistemde zaten mevcut.", description: $"Requester user name : {request.UserName}");
        //var user = _mapper.Map<User>(request);
        //var result = await _userManager.CreateAsync(user, request.Password);
        //if (!result.Succeeded)
        //    throw new GeneralException(string.Join("\n", result.Errors.Select(e => e.Description)), description: $"User cannot be created. Requester user name: {request.UserName}");

        //if (request.RoleList != null && request.RoleList.Any())
        //{
        //    var roleResult = await _userManager.AddToRolesAsync(user, request.RoleList);
        //    if (!roleResult.Succeeded)
        //        throw new GeneralException("Failed to assign role.", description: $"Requester user name: {request.UserName}");
        //}
        //return _mapper.Map<UserDto>(user);

        var result = await base.CreateAsync<UserCreateDto, UserBasicResponseDto>(request, cancellationToken);
        return result;
    }
    #endregion

    #region Update
    public async Task<Result<UserUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<UserUpdateDto>(
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );
        return result;
    }
    public async Task<Result<UserBasicResponseDto>> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.UpdateAsync<UserUpdateDto, UserBasicResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);
        return result;
        // try
        // {
        //     await _unitOfWork.BeginTransactionAsync(cancellationToken);

        //     var user = await _unitOfWork.Users.GetAsync(where: (f) => f.Id == request.Id, cancellationToken: cancellationToken);
        //     if (user == null) throw new BusinessException($"Kullanıcı bilgileri bulunamadı.");

        //     bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.OldPassword);
        //     if (!isPasswordValid) throw new BusinessException("Girdiğiniz şifre doğru değil.", description: $"Requester user name: {request.UserName}");

        //     var userToUpdate = _mapper.Map(request, user);
        //     if (!string.IsNullOrEmpty(request.NewPassword))
        //     {
        //         var resultChangePassword = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        //         if (!resultChangePassword.Succeeded) throw new GeneralException(string.Join("\n", resultChangePassword.Errors.Select(e => e.Description)), description: $"User password cannot be update. user name: {request.UserName}");
        //     }

        //     var resultUpdateUser = await _userManager.UpdateAsync(user);
        //     if (!resultUpdateUser.Succeeded) throw new GeneralException(string.Join("\n", resultUpdateUser.Errors.Select(e => e.Description)), description: $"User cannot be update. user name: {request.UserName}");

        //     // Rolleri Güncelle
        //     var existingRoles = await _userManager.GetRolesAsync(user);
        //     var requestedRoles = request.RoleList ?? new List<string>();

        //     var comparer = StringComparer.OrdinalIgnoreCase;
        //     var existingSet = new HashSet<string>(existingRoles, comparer);
        //     var requestedSet = new HashSet<string>(requestedRoles.Select(r => r.Trim()), comparer);

        //     var toAdd = requestedSet.Except(existingSet).ToList();
        //     var toRemove = existingSet.Except(requestedSet).ToList();
        //     if (toAdd.Any()) await _userManager.AddToRolesAsync(user, toAdd);
        //     if (toRemove.Any()) await _userManager.RemoveFromRolesAsync(user, toRemove);


        //     await _unitOfWork.CommitTransactionAsync(cancellationToken);

        //     return _mapper.Map<UserDto>(userToUpdate);
        // }
        // catch (Exception)
        // {
        //     await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        //     throw;    
        // }
    }
    #endregion

    #region Delete
    public async Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.DeleteAsync(where: f => f.Id == Id, cancellationToken);
        return result;
    }

    public async Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.UndoDeleteAsync(where: f => f.Id == Id, cancellationToken);
        return result;
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<User>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<PaginationResponse<UserReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync<UserReportDto>(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region Datatable
    public async Task<Result<DatatableResponseClientSide<UserReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableClientSideAsync<UserReportDto>(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<DatatableResponseServerSide<UserReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableServerSideAsync<UserReportDto>(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion
}