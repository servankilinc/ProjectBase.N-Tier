using AutoMapper;
using Business.Abstract;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Core.Utils.ResultPattern;
using Core.Utils.Validation;
using DataAccess.UoW;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.User.Commands;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public UserService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }

    #region Get
    public async Task<Result<User>> GetAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<User>.NotFound();

        return Result<User>.Success(result);
    }

    public async Task<Result<User>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<User>.NotFound();

        return Result<User>.Success(result);
    }

    public async Task<Result<UserBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAsync<UserBasicResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<UserBasicResponseDto>.NotFound();

        return Result<UserBasicResponseDto>.Success(result);
    }

    public async Task<Result<UserDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAsync<UserDetailResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<UserDetailResponseDto>.NotFound();

        return Result<UserDetailResponseDto>.Success(result);
    }

    public async Task<Result<UserBlogsResponseDto>> GetUserBlogsResponseDtoAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAsync<UserBlogsResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            include: i => i.Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<UserBlogsResponseDto>.NotFound();

        return Result<UserBlogsResponseDto>.Success(result);
    }
    #endregion

    #region Get List
    public async Task<Result<ICollection<User>>> GetListAsync(Expression<Func<User, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAllAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<User>>.NotFound();

        return Result<ICollection<User>>.Success(result);
    }

    public async Task<Result<ICollection<User>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAllAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<User>>.NotFound();

        return Result<ICollection<User>>.Success(result);
    }

    public async Task<Result<ICollection<UserBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAllAsync<UserBasicResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<UserBasicResponseDto>>.NotFound();

        return Result<ICollection<UserBasicResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<UserDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAllAsync<UserDetailResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<UserDetailResponseDto>>.NotFound();

        return Result<ICollection<UserDetailResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<UserBlogsResponseDto>>> GetUserBlogsResponseDtoListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAllAsync<UserBlogsResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<UserBlogsResponseDto>>.NotFound();

        return Result<ICollection<UserBlogsResponseDto>>.Success(result);
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<User, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Users.GetAllAsync<object>(
            select: s => new
            {
                s.Id,
                s.Name
            },
            where: where,
            cancellationToken: cancellationToken
        );
        var selectList = new SelectList(list ?? new List<object>(), "Id", "Name");

        return Result<SelectList>.Success(selectList);
    }
    #endregion

    #region Create
    public async Task<Result> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default)
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

        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Validation(validationResult.Failures, description: $"Validation failed for UserCreateDto");

        await _unitOfWork.Users.AddAndSaveAsync(_mapper.Map<User>(request), cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Update
    public async Task<Result<UserUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.GetAsync<UserUpdateDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<UserUpdateDto>.NotFound();

        return Result<UserUpdateDto>.Success(result);
    }
    public async Task<Result> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Validation(validationResult.Failures);

        var entity = await _unitOfWork.Users.GetAsync(where: f => f.Id == request.Id, cancellationToken: cancellationToken);
        if (entity == null)
            return Result.NotFound();

        await _unitOfWork.Users.UpdateAndSaveAsync(_mapper.Map(request, entity), cancellationToken);
        return Result.Success();

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
        await _unitOfWork.Users.DeleteAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success(); ;
    }

    public async Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Users.RestoreAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<UserReportDto>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.PaginationAsync<UserReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<UserReportDto>>.Success(result);
    }
    #endregion

    #region Datatable
    public async Task<Result<DatatableResponseClientSide<UserReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.DatatableClientSideAsync<UserReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseClientSide<UserReportDto>>.Success(result);
    }

    public async Task<Result<DatatableResponseServerSide<UserReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Users.DatatableServerSideAsync<UserReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseServerSide<UserReportDto>>.Success(result);
    }
    #endregion
}