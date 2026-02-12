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
using Model.Dtos.Blog.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogService : ServiceBase<Blog, IBlogRepository>, IBlogService
{
    public BlogService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper) : base(unitOfWork.Blogs, validationService, mapper)
    {
    }

    #region Get
    public async Task<Result<Blog>> GetAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<Blog>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogBasicResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogDetailResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogLikeListResponseDto>> GetBlogLikeListResponseDtoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogLikeListResponseDto>(
            where: f => f.Id == id,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region GetList
    public async Task<Result<ICollection<Blog>>> GetListAsync(Expression<Func<Blog, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<Blog>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogDetailResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogLikeListResponseDto>>> GetBlogLikeListResponseDtoListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogLikeListResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<Blog, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await base.GetListAsync(
            select: s => new
            {
                s.Id,
                s.Title
            },
            where: where,
            cancellationToken: cancellationToken
        );
        var selectList = new SelectList(list.Data ?? new List<object>(), "Id", "Title");

        return Result<SelectList>.Success(selectList);
    }
    #endregion

    #region Create
    public async Task<Result<BlogBasicResponseDto>> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.CreateAsync<BlogCreateDto, BlogBasicResponseDto>(request, cancellationToken);
        return result;
    }
    #endregion

    #region Update
    public async Task<Result<BlogBasicResponseDto>> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.UpdateAsync<BlogUpdateDto, BlogBasicResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);
        return result;
    }
    #endregion

    #region Delete
    public async Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.DeleteAsync(where: f => f.Id == Id, cancellationToken);
        return result;
    }

    public async Task<Result> UndoDeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.UndoDeleteAsync(where: f => f.Id == Id, cancellationToken);
        return result;
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<Blog>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<PaginationResponse<BlogReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync<BlogReportDto>(
            paginationRequest: request,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region Datatable
    public async Task<Result<DatatableResponseClientSide<BlogReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableClientSideAsync<BlogReportDto>(
            datatableRequest: request,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<DatatableResponseServerSide<BlogReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableServerSideAsync<BlogReportDto>(
            datatableRequest: request,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion
}
