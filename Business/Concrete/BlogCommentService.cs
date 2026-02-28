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
using Model.Dtos.BlogComment.Commands;
using Model.Dtos.BlogComment.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogCommentService : ServiceBase<BlogComment, IBlogCommentRepository>, IBlogCommentService
{
    public BlogCommentService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper) : base(unitOfWork.BlogComments, validationService, mapper)
    {
    }

    #region Get
    public async Task<Result<BlogComment>> GetAsync(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogComment>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogCommentBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogCommentBasicResponseDto>(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogCommentDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogCommentDetailResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region GetList
    public async Task<Result<ICollection<BlogComment>>> GetListAsync(Expression<Func<BlogComment, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            where: where,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogComment>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );
        return result;
    }
    public async Task<Result<ICollection<BlogCommentBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogCommentBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<ICollection<BlogCommentDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await base.GetListAsync<BlogCommentDetailResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<BlogComment, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await base.GetListAsync(
            select: s => new
            {
                s.Id,
                s.Comment
            },
            where: where,
            cancellationToken: cancellationToken
        );
        var selectList = new SelectList(list.Data ?? new List<object>(), "Id", "Comment");

        return Result<SelectList>.Success(selectList);
    }
    #endregion

    #region Create
    public async Task<Result<BlogCommentBasicResponseDto>> CreateAsync(BlogCommentCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.CreateAsync<BlogCommentCreateDto, BlogCommentBasicResponseDto>(request, cancellationToken);
        return result;
    }
    #endregion

    #region Update
    public async Task<Result<BlogCommentUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAsync<BlogCommentUpdateDto>(
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<BlogCommentBasicResponseDto>> UpdateAsync(BlogCommentUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await base.UpdateAsync<BlogCommentUpdateDto, BlogCommentBasicResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);
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
    public async Task<Result<PaginationResponse<BlogComment>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<PaginationResponse<BlogCommentReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.PaginationAsync<BlogCommentReportDto>(
            paginationRequest: request,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion

    #region Datatable
    public async Task<Result<DatatableResponseClientSide<BlogCommentReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableClientSideAsync<BlogCommentReportDto>(
            datatableRequest: request,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }

    public async Task<Result<DatatableResponseServerSide<BlogCommentReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await base.DatatableServerSideAsync<BlogCommentReportDto>(
            datatableRequest: request,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return result;
    }
    #endregion
}
