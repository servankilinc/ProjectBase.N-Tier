using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.ViewModels.BlogComment;
using Microsoft.AspNetCore.Authorization;
using Model.Dtos.BlogComment.Commands;

namespace WebUI.Controllers;

[Authorize]
public class BlogCommentController : BaseController
{
    private readonly IBlogService _blogService;
    private readonly IBlogCommentService _blogCommentService;
    private readonly IUserService _userService;
    public BlogCommentController(ILogger<BlogCommentController> logger, IBlogService blogService, IBlogCommentService blogCommentService, IUserService userService) : base(logger)
    {
        _blogService = blogService;
        _blogCommentService = blogCommentService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userIds = await _userService.SelectListAsync();
        var blogIds = await _blogService.SelectListAsync();

        var viewModel = new BlogCommentViewModel
        {
            UserIds = userIds.Data,
            BlogIds = blogIds.Data
        };

        return View(viewModel);
    }

    #region Create 
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var userIds = await _userService.SelectListAsync();
        var blogIds = await _blogService.SelectListAsync();

        var viewModel = new BlogCommentCreateViewModel
        {
            UserIds = userIds.Data,
            BlogIds = blogIds.Data
        };

        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BlogCommentCreateDto createModel)
    {
        var result = await _blogCommentService.CreateAsync(createModel);
        return ToAction(result);
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await _blogCommentService.GetUpdateModelAsync(id);
        if (!result.IsSuccess) return ToAction(result);

        var userIds = await _userService.SelectListAsync();
        var blogIds = await _blogService.SelectListAsync();

        var viewModel = new BlogCommentUpdateViewModel
        {
            UpdateModel = result.Data,
            UserIds = userIds.Data,
            BlogIds = blogIds.Data
        };

        return PartialView("./Partials/UpdateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BlogCommentUpdateDto updateModel)
    {
        var result = await _blogCommentService.UpdateAsync(updateModel);
        return ToAction(result);
    }
    #endregion

    #region Delete
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _blogCommentService.DeleteAsync(id);
        return ToAction(result);
    }

    [HttpGet]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _blogCommentService.UndoDeleteAsync(id);
        return ToAction(result);
    }
    #endregion

    #region Datatable
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _blogCommentService.DatatableClientSideAsync(request);
        return ToAction(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _blogCommentService.DatatableServerSideAsync(request);
        return ToAction(result);
    }
    #endregion
}
