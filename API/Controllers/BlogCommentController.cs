using API.Controllers.Base;
using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.BlogComment.Commands;

namespace API.Controllers;

[Authorize]
public class BlogCommentController : BaseController
{
    private readonly IBlogCommentService _blogCommentService;
    public BlogCommentController(ILogger<BlogCommentController> logger, IBlogCommentService blogCommentService) : base(logger) => _blogCommentService = blogCommentService;


    #region Get
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _blogCommentService.GetAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/base")]
    public async Task<IActionResult> GetBasic(Guid id)
    {
        var result = await _blogCommentService.GetBasicAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/detail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _blogCommentService.GetDetailAsync(id);
        return FromResult(result);
    }
    #endregion

    #region GetList
    [HttpPost("list")]
    public async Task<IActionResult> GetList(DynamicRequest? request)
    {
        var result = await _blogCommentService.GetListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/base")]
    public async Task<IActionResult> GetBasicList(DynamicRequest request)
    {
        var result = await _blogCommentService.GetBasicListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/detail")]
    public async Task<IActionResult> GetDetailList(DynamicRequest? request)
    {
        var result = await _blogCommentService.GetDetailListAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(BlogCommentCreateDto request)
    {
        var result = await _blogCommentService.CreateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update(BlogCommentUpdateDto request)
    {
        var result = await _blogCommentService.UpdateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _blogCommentService.DeleteAsync(id);
        return FromResult(result);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _blogCommentService.UndoDeleteAsync(id);
        return FromResult(result);
    }
    #endregion

    #region Pagination
    [HttpPost("pagination")]
    public async Task<IActionResult> Pagination(DynamicPaginationRequest request)
    {
        var result = await _blogCommentService.PaginationAsync(request);
        return FromResult(result);
    }

    [HttpPost("pagination/report")]
    public async Task<IActionResult> PaginationReport(DynamicPaginationRequest request)
    {
        var result = await _blogCommentService.PaginationReportAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Datatable
    [HttpPost("datatable/client")]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _blogCommentService.DatatableClientSideAsync(request);
        return FromResult(result);
    }

    [HttpPost("datatable/server")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _blogCommentService.DatatableServerSideAsync(request);
        return FromResult(result);
    }
    #endregion
}
