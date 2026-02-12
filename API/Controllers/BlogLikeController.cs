using API.Controllers.Base;
using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.BlogLike.Commands;
using Model.Entities;

namespace API.Controllers;

[Authorize]
public class BlogLikeController : BaseController
{
    private readonly IBlogLikeService _blogLikeService;
    public BlogLikeController(ILogger<BlogLikeController> logger, IBlogLikeService blogLikeService) : base(logger) => _blogLikeService = blogLikeService;


    #region Get
    [HttpGet]
    public async Task<IActionResult> Get(Guid blogId, Guid userId)
    {
        var result = await _blogLikeService.GetAsync(BlogId: blogId, UserId: userId);
        return FromResult(result);
    }

    [HttpGet("base")]
    public async Task<IActionResult> GetBasic(Guid blogId, Guid userId)
    {
        var result = await _blogLikeService.GetBasicAsync(BlogId: blogId, UserId: userId);
        return FromResult(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetDetail(Guid blogId, Guid userId)
    {
        var result = await _blogLikeService.GetDetailAsync(BlogId: blogId, UserId: userId);
        return FromResult(result);
    }
    #endregion

    #region GetList
    [HttpPost("list")]
    public async Task<IActionResult> GetList(DynamicRequest? request)
    {
        var result = await _blogLikeService.GetListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/base")]
    public async Task<IActionResult> GetBasicList(DynamicRequest request)
    {
        var result = await _blogLikeService.GetBasicListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/detail")]
    public async Task<IActionResult> GetDetailList(DynamicRequest? request)
    {
        var result = await _blogLikeService.GetDetailListAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(BlogLikeCreateDto request)
    {
        var result = await _blogLikeService.CreateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update(BlogLike request)
    {
        var result = await _blogLikeService.UpdateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Pagination
    [HttpPost("pagination")]
    public async Task<IActionResult> Pagination(DynamicPaginationRequest request)
    {
        var result = await _blogLikeService.PaginationAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Delete
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(BlogLikeDeleteDto request)
    {
        await _blogLikeService.DeleteAsync(request);

        return Ok();
    }
    #endregion

    #region Datatable
    [HttpPost("datatable/client")]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _blogLikeService.DatatableClientSideAsync(request);
        return FromResult(result);
    }

    [HttpPost("datatable/server")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _blogLikeService.DatatableServerSideAsync(request);
        return FromResult(result);
    }
    #endregion
}
