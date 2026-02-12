using Business.Abstract;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog_;
using Model.Dtos.Category_;
using Model.Dtos.User_;
using WebUI.Models.ViewModels.Blog;
using WebUI.Models.ViewModels.User;
using WebUI.Utils.ActionFilters;

namespace WebUI.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;
    // private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    // private readonly UserManager<User> _userManager;
    public UserController(IUserService userService)
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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new UserCreateViewModel
        {
        };
        return View(viewModel);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<UserCreateDto>))]
    public async Task<IActionResult> Create(UserCreateDto createModel)
    {
        var result = await _userService.CreateAsync(createModel);
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<UserUpdateDto>))]
    public async Task<IActionResult> Update(UserUpdateDto updateModel)
    {
        var result = await _userService.UpdateAsync(updateModel);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> UndoDelete(Guid id)
    {
        if (id == default) throw new ArgumentNullException(nameof(id));
        await _userService._UndoDeleteAsync(f => f.Id == id);
        return Ok();
    }

    #region Datatable
    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _userService.DatatableServerSideByReportAsync(request);
        return Ok(result);
    }
    #endregion

    #region Form Partials
    [HttpGet]
    public async Task<IActionResult> CreateForm()
    {
        // var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();    
        // RoleSelectList = allRoles.Select(r => new SelectListItem(r, r)).ToList()
        var viewModel = new UserCreateViewModel
        {
        };
        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateForm(Guid id)
    {
        var data = await _userService.GetAsync<UserUpdateDto>(where: f => f.Id == id);
        if (data == null) return NotFound(data);
        
        // var user = await _userManager.FindByIdAsync(model.Id.ToString());
        // if (user == null) return NotFound(user);
        // var existRoles = await _userManager.GetRolesAsync(user);            
        // var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        // RoleSelectList = allRoles.Select(r => new SelectListItem(r, r, r != null && existRoles.Contains(r))).ToList(),
               
        var viewModel = new UserUpdateViewModel
        {
            UpdateModel = data
        };
        return PartialView("./Partials/UpdateForm", viewModel);
    }
    #endregion
}
