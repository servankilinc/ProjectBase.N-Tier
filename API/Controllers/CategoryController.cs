using API.Controllers.Base;
using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Category.Commands;

namespace API.Controllers;

[Authorize]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger) => _categoryService = categoryService;

    #region Get 
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _categoryService.GetAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/base")]
    public async Task<IActionResult> GetBasic(Guid id)
    {
        var result = await _categoryService.GetBasicAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/detail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _categoryService.GetDetailAsync(id);
        return FromResult(result);
    }
    #endregion

    #region GetList
    [HttpPost("list")]
    public async Task<IActionResult> GetList(DynamicRequest? request)
    {
        var result = await _categoryService.GetListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/base")]
    public async Task<IActionResult> GetBasicList(DynamicRequest request)
    {
        var result = await _categoryService.GetBasicListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/detail")]
    public async Task<IActionResult> GetDetailList(DynamicRequest? request)
    {
        var result = await _categoryService.GetDetailListAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto request)
    {
        var result = await _categoryService.CreateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update(CategoryUpdateDto request)
    {
        var result = await _categoryService.UpdateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return FromResult(result);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _categoryService.UndoDeleteAsync(id);
        return FromResult(result);
    }
    #endregion

    #region Pagination
    [HttpPost("pagination")]
    public async Task<IActionResult> Pagination(DynamicPaginationRequest request)
    {
        var result = await _categoryService.PaginationAsync(request);
        return FromResult(result);
    }

    [HttpPost("pagination/report")]
    public async Task<IActionResult> PaginationReport(DynamicPaginationRequest request)
    {
        var result = await _categoryService.PaginationReportAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Datatable Methods
    [HttpPost("datatable/client")]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _categoryService.DatatableClientSideAsync(request);
        return FromResult(result);
    }

    [HttpPost("datatable/server")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _categoryService.DatatableServerSideAsync(request);
        return FromResult(result);
    }
    #endregion
}
