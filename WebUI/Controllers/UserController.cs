using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.User.Commands;
using WebUI.Models.ViewModels.User;

namespace WebUI.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;
    // private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    // private readonly UserManager<User> _userManager;
    public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new UserViewModel
        {
        };
        return View(viewModel);
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();    
        // RoleSelectList = allRoles.Select(r => new SelectListItem(r, r)).ToList()
        var viewModel = new UserCreateViewModel
        {
        };
        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto createModel)
    {
        var result = await _userService.CreateAsync(createModel);
        return ToAction(result);
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await _userService.GetUpdateModelAsync(id);
        if (!result.IsSuccess) return ToAction(result);

        // var user = await _userManager.FindByIdAsync(model.Id.ToString());
        // if (user == null) return NotFound(user);
        // var existRoles = await _userManager.GetRolesAsync(user);            
        // var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        // RoleSelectList = allRoles.Select(r => new SelectListItem(r, r, r != null && existRoles.Contains(r))).ToList(),

        var viewModel = new UserUpdateViewModel
        {
            UpdateModel = result.Data
        };

        return PartialView("./Partials/UpdateForm", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserUpdateDto updateModel)
    {
        var result = await _userService.UpdateAsync(updateModel);
        return ToAction(result);
    }
    #endregion

    #region Delete
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        return ToAction(result);
    }

    [HttpGet]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _userService.UndoDeleteAsync(id);
        return ToAction(result);
    }
    #endregion

    #region Datatable
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _userService.DatatableClientSideAsync(request);
        return ToAction(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _userService.DatatableServerSideAsync(request);
        return ToAction(result);
    }
    #endregion
}
