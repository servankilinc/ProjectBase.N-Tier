using API.Controllers.Base;
using Business.Abstract;
using Business.Concrete;
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
        return ToAction(result);
    }

    [HttpGet("{id:guid}/base")]
    public async Task<IActionResult> GetBasic(Guid id)
    {
        var result = await _blogService.GetBasicAsync(id);
        return ToAction(result);
    }

    [HttpGet("{id:guid}/detail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _blogService.GetDetailAsync(id);
        return ToAction(result);
    }

    [HttpGet("{id:guid}/BlogLikeListResponseDto")]
    public async Task<IActionResult> GetBlogLikeListResponseDto(Guid id)
    {
        var result = await _blogService.GetBlogLikeListResponseDtoAsync(id);
        return ToAction(result);
    }
    #endregion

    #region GetList
    [HttpPost("list")]
    public async Task<IActionResult> GetList(DynamicRequest? request)
    {
        var result = await _blogService.GetListAsync(request);
        return ToAction(result);
    }

    [HttpPost("list/base")]
    public async Task<IActionResult> GetBasicList(DynamicRequest request)
    {
        var result = await _blogService.GetBasicListAsync(request);
        return ToAction(result);
    }

    [HttpPost("list/detail")]
    public async Task<IActionResult> GetDetailList(DynamicRequest? request)
    {
        var result = await _blogService.GetDetailListAsync(request);
        return ToAction(result);
    }

    [HttpPost("list/BlogLikeListResponseDto")]
    public async Task<IActionResult> GetBlogLikeListResponseDtoList(DynamicRequest? request)
    {
        var result = await _blogService.GetBlogLikeListResponseDtoListAsync(request);
        return ToAction(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateDto request)
    {
        var result = await _blogService.CreateAsync(request);
        return ToAction(result);
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await _blogService.GetUpdateModelAsync(id);
        return ToAction(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(BlogUpdateDto request)
    {
        var result = await _blogService.UpdateAsync(request);
        return ToAction(result);
    }
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _blogService.DeleteAsync(id);
        return ToAction(result);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _blogService.UndoDeleteAsync(id);
        return ToAction(result);
    }
    #endregion

    #region Pagination
    [HttpPost("pagination")]
    public async Task<IActionResult> Pagination(DynamicPaginationRequest request)
    {
        var result = await _blogService.PaginationAsync(request);
        return ToAction(result);
    }

    [HttpPost("pagination/report")]
    public async Task<IActionResult> PaginationReport(DynamicPaginationRequest request)
    {
        var result = await _blogService.PaginationReportAsync(request);
        return ToAction(result);
    }
    #endregion

    #region Datatable
    [HttpPost("datatable/client")]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _blogService.DatatableClientSideAsync(request);
        return ToAction(result);
    }

    [HttpPost("datatable/server")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _blogService.DatatableServerSideAsync(request);
        return ToAction(result);
    }
    #endregion
}
