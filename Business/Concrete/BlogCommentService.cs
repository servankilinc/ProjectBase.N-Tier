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
using Model.Dtos.BlogComment.Commands;
using Model.Dtos.BlogComment.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogCommentService : IBlogCommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public BlogCommentService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }

    #region Get
    public async Task<Result<BlogComment>> GetAsync(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogComment>.NotFound();

        return Result<BlogComment>.Success(result);
    }

    public async Task<Result<BlogComment>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogComment>.NotFound();

        return Result<BlogComment>.Success(result);
    }

    public async Task<Result<BlogCommentBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAsync<BlogCommentBasicResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogCommentBasicResponseDto>.NotFound();

        return Result<BlogCommentBasicResponseDto>.Success(result);
    }

    public async Task<Result<BlogCommentDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAsync<BlogCommentDetailResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogCommentDetailResponseDto>.NotFound();

        return Result<BlogCommentDetailResponseDto>.Success(result);
    }
    #endregion

    #region GetList
    public async Task<Result<ICollection<BlogComment>>> GetListAsync(Expression<Func<BlogComment, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAllAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogComment>>.NotFound();

        return Result<ICollection<BlogComment>>.Success(result);
    }

    public async Task<Result<ICollection<BlogComment>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAllAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogComment>>.NotFound();

        return Result<ICollection<BlogComment>>.Success(result);
    }
    public async Task<Result<ICollection<BlogCommentBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAllAsync<BlogCommentBasicResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogCommentBasicResponseDto>>.NotFound();

        return Result<ICollection<BlogCommentBasicResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<BlogCommentDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAllAsync<BlogCommentDetailResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogCommentDetailResponseDto>>.NotFound();

        return Result<ICollection<BlogCommentDetailResponseDto>>.Success(result);
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<BlogComment, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.BlogComments.GetAllAsync<object>(
            select: s => new
            {
                s.Id,
                s.Comment
            },
            where: where,
            cancellationToken: cancellationToken
        );
        var selectList = new SelectList(list ?? new List<object>(), "Id", "Comment");

        return Result<SelectList>.Success(selectList);
    }
    #endregion

    #region Create
    public async Task<Result<BlogCommentBasicResponseDto>> CreateAsync(BlogCommentCreateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<BlogCommentBasicResponseDto>.Validation(validationResult.Failures, description: $"Validation failed for BlogCommentBasicResponseDto");

        var result = await _unitOfWork.BlogComments.AddAndSaveAsync(_mapper.Map<BlogComment>(request), cancellationToken);
        return Result<BlogCommentBasicResponseDto>.Success(_mapper.Map<BlogCommentBasicResponseDto>(result));
    }
    #endregion

    #region Update
    public async Task<Result<BlogCommentUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.GetAsync<BlogCommentUpdateDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogCommentUpdateDto>.NotFound();

        return Result<BlogCommentUpdateDto>.Success(result);
    }

    public async Task<Result<BlogCommentBasicResponseDto>> UpdateAsync(BlogCommentUpdateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<BlogCommentBasicResponseDto>.Validation(validationResult.Failures);

        var entity = await _unitOfWork.BlogComments.GetAsync(where: f => f.Id == request.Id, cancellationToken: cancellationToken);
        if (entity == null)
            return Result<BlogCommentBasicResponseDto>.NotFound();

        var result = await _unitOfWork.BlogComments.UpdateAndSaveAsync(_mapper.Map(request, entity), cancellationToken);
        return Result<BlogCommentBasicResponseDto>.Success(_mapper.Map<BlogCommentBasicResponseDto>(result));
    }
    #endregion

    #region Delete
    public async Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BlogComments.DeleteAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BlogComments.RestoreAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<BlogComment>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<BlogComment>>.Success(result);
    }

    public async Task<Result<PaginationResponse<BlogCommentReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.PaginationAsync<BlogCommentReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            paginationRequest: request,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<BlogCommentReportDto>>.Success(result);
    }
    #endregion

    #region Datatable
    public async Task<Result<DatatableResponseClientSide<BlogCommentReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.DatatableClientSideAsync<BlogCommentReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseClientSide<BlogCommentReportDto>>.Success(result);
    }

    public async Task<Result<DatatableResponseServerSide<BlogCommentReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.BlogComments.DatatableServerSideAsync<BlogCommentReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseServerSide<BlogCommentReportDto>>.Success(result);
    }
    #endregion
}
