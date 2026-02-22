using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog.Commands;
using WebUI.Models.ViewModels.Blog;

namespace WebUI.Controllers;

[Authorize]
public class BlogController : BaseController
{
    private readonly IBlogService _blogService;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;
    public BlogController(ILogger<BlogController> logger, IBlogService blogService, ICategoryService categoryService, IUserService userService) : base(logger)
    {
        _blogService = blogService;
        _categoryService = categoryService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var authorIds = await _userService.SelectListAsync();
        var categoryIds = await _categoryService.SelectListAsync();

        var viewModel = new BlogViewModel
        {
            AuthorIds = authorIds.Data,
            CategoryIds = categoryIds.Data
        };

        return View(viewModel);
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var authorIds = await _userService.SelectListAsync();
        var categoryIds = await _categoryService.SelectListAsync();

        var viewModel = new BlogCreateViewModel
        {
            AuthorIds = authorIds.Data,
            CategoryIds = categoryIds.Data
        };

        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateDto createModel)
    {
        var result = await _blogService.CreateAsync(createModel);
        return ToAction(result);
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await _blogService.GetUpdateModelAsync(id);
        if (!result.IsSuccess) return ToAction(result);

        var authorIds = await _userService.SelectListAsync();
        var categoryIds = await _categoryService.SelectListAsync();

        var viewModel = new BlogUpdateViewModel
        {
            UpdateModel = result.Data,
            AuthorIds = authorIds.Data,
            CategoryIds = categoryIds.Data
        };

        return PartialView("./Partials/UpdateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BlogUpdateDto updateModel)
    {
        var result = await _blogService.UpdateAsync(updateModel);
        return ToAction(result);
    }
    #endregion

    #region Delete
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _blogService.DeleteAsync(id);
        return ToAction(result);
    }


    [HttpGet]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _blogService.UndoDeleteAsync(id);
        return ToAction(result);
    }
    #endregion

    #region Datatable
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _blogService.DatatableClientSideAsync(request);
        return ToAction(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _blogService.DatatableServerSideAsync(request);
        return ToAction(result);
    }
    #endregion
}