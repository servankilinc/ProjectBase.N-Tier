using Business.Abstract;
using Business.Concrete;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Category.Commands;
using WebUI.Models.ViewModels.Category;

namespace WebUI.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) :base(logger)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new CategoryViewModel
        {
        };

        return View(viewModel);
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CategoryCreateViewModel
        {
        };

        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto createModel)
    {
        var result = await _categoryService.CreateAsync(createModel);
        return ToAction(result);
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await _categoryService.GetUpdateModelAsync(id);
        if (!result.IsSuccess) return ToAction(result);

        var viewModel = new CategoryUpdateViewModel
        {
            UpdateModel = result.Data
        };

        return PartialView("./Partials/UpdateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateDto updateModel)
    {
        var result = await _categoryService.UpdateAsync(updateModel);
        return ToAction(result);
    }
    #endregion

    #region Delete
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return ToAction(result);
    }

    [HttpGet]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _categoryService.UndoDeleteAsync(id);
        return ToAction(result);
    }
    #endregion

    #region Datatable
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _categoryService.DatatableClientSideAsync(request);
        return ToAction(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _categoryService.DatatableServerSideAsync(request);
        return ToAction(result);
    }
    #endregion
}