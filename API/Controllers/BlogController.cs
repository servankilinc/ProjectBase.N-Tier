using API.Controllers.Base;
using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog.Commands;

namespace API.Controllers;

[Authorize]
public class BlogController : BaseController
{
    private readonly IBlogService _blogService;
    public BlogController(ILogger<BlogController> logger, IBlogService blogService) : base(logger) => _blogService = blogService;


    #region Get
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _blogService.GetAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/base")]
    public async Task<IActionResult> GetBasic(Guid id)
    {
        var result = await _blogService.GetBasicAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/detail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _blogService.GetDetailAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/BlogLikeListResponseDto")]
    public async Task<IActionResult> GetBlogLikeListResponseDto(Guid id)
    {
        var result = await _blogService.GetBlogLikeListResponseDtoAsync(id);
        return FromResult(result);
    }
    #endregion

    #region GetList
    [HttpPost("list")]
    public async Task<IActionResult> GetList(DynamicRequest? request)
    {
        var result = await _blogService.GetListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/base")]
    public async Task<IActionResult> GetBasicList(DynamicRequest request)
    {
        var result = await _blogService.GetBasicListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/detail")]
    public async Task<IActionResult> GetDetailList(DynamicRequest? request)
    {
        var result = await _blogService.GetDetailListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/BlogLikeListResponseDto")]
    public async Task<IActionResult> GetBlogLikeListResponseDtoList(DynamicRequest? request)
    {
        var result = await _blogService.GetBlogLikeListResponseDtoListAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateDto request)
    {
        var result = await _blogService.CreateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update(BlogUpdateDto request)
    {
        var result = await _blogService.UpdateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _blogService.DeleteAsync(id);
        return FromResult(result);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _blogService.UndoDeleteAsync(id);
        return FromResult(result);
    }
    #endregion

    #region Pagination
    [HttpPost("pagination")]
    public async Task<IActionResult> Pagination(DynamicPaginationRequest request)
    {
        var result = await _blogService.PaginationAsync(request);
        return FromResult(result);
    }

    [HttpPost("pagination/report")]
    public async Task<IActionResult> PaginationReport(DynamicPaginationRequest request)
    {
        var result = await _blogService.PaginationReportAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Datatable
    [HttpPost("datatable/client")]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _blogService.DatatableClientSideAsync(request);
        return FromResult(result);
    }

    [HttpPost("datatable/server")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _blogService.DatatableServerSideAsync(request);
        return FromResult(result);
    }
    #endregion
}
