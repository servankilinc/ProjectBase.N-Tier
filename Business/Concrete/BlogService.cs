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
using Model.Dtos.Blog.Commands;
using Model.Dtos.Blog.Queries;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogService : IBlogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public BlogService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }

    #region Get
    public async Task<Result<Blog>> GetAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<Blog>.NotFound();

        return Result<Blog>.Success(result);
    }

    public async Task<Result<Blog>> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAsync(
            where: f => f.Id == Id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<Blog>.NotFound();

        return Result<Blog>.Success(result);
    }

    public async Task<Result<BlogBasicResponseDto>> GetBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAsync<BlogBasicResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogBasicResponseDto>.NotFound();

        return Result<BlogBasicResponseDto>.Success(result);
    }

    public async Task<Result<BlogDetailResponseDto>> GetDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAsync<BlogDetailResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogDetailResponseDto>.NotFound();

        return Result<BlogDetailResponseDto>.Success(result);
    }

    public async Task<Result<BlogLikeListResponseDto>> GetBlogLikeListResponseDtoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAsync<BlogLikeListResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == id,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogLikeListResponseDto>.NotFound();

        return Result<BlogLikeListResponseDto>.Success(result);
    }
    #endregion

    #region GetList
    public async Task<Result<ICollection<Blog>>> GetListAsync(Expression<Func<Blog, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAllAsync(
            where: where,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<Blog>>.NotFound();

        return Result<ICollection<Blog>>.Success(result);
    }

    public async Task<Result<ICollection<Blog>>> GetListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAllAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<Blog>>.NotFound();

        return Result<ICollection<Blog>>.Success(result);
    }

    public async Task<Result<ICollection<BlogBasicResponseDto>>> GetBasicListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAllAsync<BlogBasicResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogBasicResponseDto>>.NotFound();

        return Result<ICollection<BlogBasicResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<BlogDetailResponseDto>>> GetDetailListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAllAsync<BlogDetailResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogDetailResponseDto>>.NotFound();

        return Result<ICollection<BlogDetailResponseDto>>.Success(result);
    }

    public async Task<Result<ICollection<BlogLikeListResponseDto>>> GetBlogLikeListResponseDtoListAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAllAsync<BlogLikeListResponseDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User),
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<ICollection<BlogLikeListResponseDto>>.NotFound();

        return Result<ICollection<BlogLikeListResponseDto>>.Success(result);
    }
    #endregion

    #region SelectList
    public async Task<Result<SelectList>> SelectListAsync(Expression<Func<Blog, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Blogs.GetAllAsync<object>(
            select: s => new
            {
                s.Id,
                s.Title
            },
            where: where,
            cancellationToken: cancellationToken
        );
        var selectList = new SelectList(list ?? new List<object>(), "Id", "Title");

        return Result<SelectList>.Success(selectList);
    }
    #endregion

    #region Create
    public async Task<Result<BlogBasicResponseDto>> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<BlogBasicResponseDto>.Validation(validationResult.Failures, description: $"Validation failed for BlogCommentBasicResponseDto");

        var result = await _unitOfWork.Blogs.AddAndSaveAsync(_mapper.Map<Blog>(request), cancellationToken);
        return Result<BlogBasicResponseDto>.Success(_mapper.Map<BlogBasicResponseDto>(result));
    }
    #endregion

    #region Update
    public async Task<Result<BlogUpdateDto>> GetUpdateModelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.GetAsync<BlogUpdateDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            where: f => f.Id == id,
            cancellationToken: cancellationToken
        );

        if (result == null)
            return Result<BlogUpdateDto>.NotFound();

        return Result<BlogUpdateDto>.Success(result);
    }

    public async Task<Result<BlogBasicResponseDto>> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<BlogBasicResponseDto>.Validation(validationResult.Failures);

        var entity = await _unitOfWork.Blogs.GetAsync(where: f => f.Id == request.Id, cancellationToken: cancellationToken);
        if (entity == null)
            return Result<BlogBasicResponseDto>.NotFound();

        var result = await _unitOfWork.Blogs.UpdateAndSaveAsync(_mapper.Map(request, entity), cancellationToken);
        return Result<BlogBasicResponseDto>.Success(_mapper.Map<BlogBasicResponseDto>(result));
    }
    #endregion

    #region Delete
    public async Task<Result> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Blogs.DeleteAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RestoreAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Blogs.RestoreAndSaveAsync(where: f => f.Id == Id, cancellationToken);
        return Result.Success();
    }
    #endregion

    #region Pagination
    public async Task<Result<PaginationResponse<Blog>>> PaginationAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.PaginationAsync(
            paginationRequest: request,
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<Blog>>.Success(result);
    }

    public async Task<Result<PaginationResponse<BlogReportDto>>> PaginationReportAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.PaginationAsync<BlogReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            paginationRequest: request,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return Result<PaginationResponse<BlogReportDto>>.Success(result);
    }
    #endregion

    #region Datatable
    public async Task<Result<DatatableResponseClientSide<BlogReportDto>>> DatatableClientSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.DatatableClientSideAsync<BlogReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseClientSide<BlogReportDto>>.Success(result);
    }

    public async Task<Result<DatatableResponseServerSide<BlogReportDto>>> DatatableServerSideAsync(DynamicDatatableRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Blogs.DatatableServerSideAsync<BlogReportDto>(
            configurationProvider: _mapper.ConfigurationProvider,
            datatableRequest: request,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );
        return Result<DatatableResponseServerSide<BlogReportDto>>.Success(result);
    }
    #endregion
}
