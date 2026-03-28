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
using Model.Dtos.Category.Commands;
using Model.Dtos.Category.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public CategoryService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }

    #region Get
    public async Task<Result<Category>> GetAsync(Expression<Func<Category, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<Category>.NotFound();

        return Result<Category>.Success(result);
    }

    public async Task<Result<Category>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<Category>.NotFound();

        return Result<Category>.Success(result);
    }

    public async Task<Result<CategoryResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAsync<CategoryResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<CategoryResponseDto>.NotFound();

        return Result<CategoryResponseDto>.Success(result);
    }

    public async Task<Result<CategoryBlogsResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAsync<CategoryBlogsResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<CategoryBlogsResponseDto>.NotFound();

        return Result<CategoryBlogsResponseDto>.Success(result);
    }
    #endregion

    #region GetList
    public async Task<Result<ICollection<Category>>> GetListAsync(Expression<Func<Category, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAllAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<Category>>.NotFound();

        return Result<ICollection<Category>>.Success(result);
    }

    public async Task<Result<ICollection<Category>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAllAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<Category>>.NotFound();

        return Result<ICollection<Category>>.Success(result);
    }

    public async Task<Result<ICollection<CategoryResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAllAsync<CategoryResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<CategoryResponseDto>>.NotFound();

        return Result<ICollection<CategoryResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<CategoryBlogsResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAllAsync<CategoryBlogsResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<CategoryBlogsResponseDto>>.NotFound();

        return Result<ICollection<CategoryBlogsResponseDto>>.Success(result);
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<Category, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Categories.GetAllAsync<object>(
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
    public async Task<Result<CategoryResponseDto>> CreateAsync(CategoryCreateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<CategoryResponseDto>.Validation(validationResult.Failures, description: $"Validation failed for CategoryResponseDto");

        var result = await _unitOfWork.Categories.AddAndSaveAsync(_mapper.Map<Category>(request), cancellationToken);
        return Result<CategoryResponseDto>.Success(_mapper.Map<CategoryResponseDto>(result));
    }
    #endregion

    #region Update
    public async Task<Result<CategoryUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.GetAsync<CategoryUpdateDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<CategoryUpdateDto>.NotFound();

        return Result<CategoryUpdateDto>.Success(result);
    }
    public async Task<Result<CategoryResponseDto>> UpdateAsync(CategoryUpdateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<CategoryResponseDto>.Validation(validationResult.Failures);

        var entity = await _unitOfWork.Categories.GetAsync(where: f => f.Id == request.Id, cancellationToken: cancellationToken);
        if (entity == null)
            return Result<CategoryResponseDto>.NotFound();

        var result = await _unitOfWork.Categories.UpdateAndSaveAsync(_mapper.Map(request, entity), cancellationToken);
        return Result<CategoryResponseDto>.Success(_mapper.Map<CategoryResponseDto>(result));
    }
    #endregion

    #region Delete
    public async Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Categories.DeleteAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Categories.RestoreAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<Category>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<Category>>.Success(result);
    }

    public async Task<Result<PaginationResponse<CategoryReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.PaginationAsync<CategoryReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<CategoryReportDto>>.Success(result);
    }
    #endregion

    #region Datatable Methods
    public async Task<Result<DatatableResponseClientSide<CategoryReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.DatatableClientSideAsync<CategoryReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseClientSide<CategoryReportDto>>.Success(result);
    }

    public async Task<Result<DatatableResponseServerSide<CategoryReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Categories.DatatableServerSideAsync<CategoryReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseServerSide<CategoryReportDto>>.Success(result);
    }
    #endregion
}
