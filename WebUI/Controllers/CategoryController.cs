using Business.Abstract;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Category_;
using WebUI.Models.ViewModels.Category;
using WebUI.Utils.ActionFilters;

namespace WebUI.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CategoryCreateViewModel
        {
        };

        return View(viewModel);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CategoryCreateDto>))]
    public async Task<IActionResult> Create(CategoryCreateDto createModel)
    {
        var result = await _categoryService.CreateAsync(createModel);
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CategoryUpdateDto>))]
    public async Task<IActionResult> Update(CategoryUpdateDto updateModel)
    {
        var result = await _categoryService.UpdateAsync(updateModel);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok();
    }

    #region Datatable
    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _categoryService.DatatableServerSideByReportAsync(request);
        return Ok(result);
    }
    #endregion

    #region Form Partials
    [HttpGet]
    public async Task<IActionResult> CreateForm()
    {
        var viewModel = new CategoryCreateViewModel
        {
        };

        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateForm(Guid id)
    {
        var data = await _categoryService.GetAsync<CategoryUpdateDto>(where: f => f.Id == id);

        if (data == null) return NotFound(data);

        var viewModel = new CategoryUpdateViewModel
        {
            UpdateModel = data
        };
        return PartialView("./Partials/UpdateForm", viewModel);
    }
    #endregion
}
