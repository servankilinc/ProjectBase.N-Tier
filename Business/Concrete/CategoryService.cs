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
using Model.Dtos.Blog.Commands;
using Model.Dtos.Category.Commands;
using Model.Dtos.Category.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class CategoryService : ServiceBase<Category, ICategoryRepository>, ICategoryService
{
    public CategoryService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper) : base(unitOfWork.Categories, validationService, mapper)
    {
    }

    #region Get
    public async Task<Result<Category>> GetAsync(Expression<Func<Category, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<Category>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<CategoryResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<CategoryResponseDto>(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<CategoryBlogsResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<CategoryBlogsResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region GetList
    public async Task<Result<ICollection<Category>>> GetListAsync(Expression<Func<Category, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<Category>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<CategoryResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<CategoryResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<CategoryBlogsResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<CategoryBlogsResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<Category, bool>>? where = default, CancellationToken cancellationToken = default)
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
    public async Task<Result<CategoryResponseDto>> CreateAsync(CategoryCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.CreateAsync<CategoryCreateDto, CategoryResponseDto>(request, cancellationToken);
        return result;
    }
    #endregion

    #region Update
    public async Task<Result<CategoryUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<CategoryUpdateDto>(
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );
        return result;
    }
    public async Task<Result<CategoryResponseDto>> UpdateAsync(CategoryUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.UpdateAsync<CategoryUpdateDto, CategoryResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);
        return result;
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
    public async Task<Result<PaginationResponse<Category>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<PaginationResponse<CategoryReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync<CategoryReportDto>(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region Datatable Methods
    public async Task<Result<DatatableResponseClientSide<CategoryReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableClientSideAsync<CategoryReportDto>(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<DatatableResponseServerSide<CategoryReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableServerSideAsync<CategoryReportDto>(
            datatableRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion
}
