using API.Controllers.Base;
using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.User.Queries;

namespace API.Controllers;

[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    public UserController(ILogger<UserController> logger, IUserService userService) : base(logger) => _userService = userService;

    #region Get
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _userService.GetAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/base")]
    public async Task<IActionResult> GetBasic(Guid id)
    {
        var result = await _userService.GetBasicAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/detail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _userService.GetDetailAsync(id);
        return FromResult(result);
    }

    [HttpGet("{id:guid}/UserBlogsResponseDto")]
    public async Task<IActionResult> GetUserBlogsResponseDto(Guid id)
    {
        var result = await _userService.GetUserBlogsResponseDtoAsync(id);
        return FromResult(result);
    }
    #endregion

    #region GetList
    [HttpPost("list")]
    public async Task<IActionResult> GetList(DynamicRequest? request)
    {
        var result = await _userService.GetListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/base")]
    public async Task<IActionResult> GetBasicList(DynamicRequest request)
    {
        var result = await _userService.GetBasicListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/detail")]
    public async Task<IActionResult> GetDetailList(DynamicRequest? request)
    {
        var result = await _userService.GetDetailListAsync(request);
        return FromResult(result);
    }

    [HttpPost("list/UserBlogsResponseDto")]
    public async Task<IActionResult> GetUserBlogsResponseDtoList(DynamicRequest? request)
    {
        var result = await _userService.GetUserBlogsResponseDtoListAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto request)
    {
        var result = await _userService.CreateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update(UserUpdateDto request)
    {
        var result = await _userService.UpdateAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        return FromResult(result);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _userService.UndoDeleteAsync(id);
        return FromResult(result);
    }
    #endregion

    #region Pagination
    [HttpPost("pagination")]
    public async Task<IActionResult> Pagination(DynamicPaginationRequest request)
    {
        var result = await _userService.PaginationAsync(request);
        return FromResult(result);
    }

    [HttpPost("pagination/report")]
    public async Task<IActionResult> PaginationReport(DynamicPaginationRequest request)
    {
        var result = await _userService.PaginationReportAsync(request);
        return FromResult(result);
    }
    #endregion

    #region Datatable
    [HttpPost("datatable/client")]
    public async Task<IActionResult> DatatableClientSide(DynamicDatatableRequest request)
    {
        var result = await _userService.DatatableClientSideAsync(request);
        return FromResult(result);
    }

    [HttpPost("datatable/server")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableRequest request)
    {
        var result = await _userService.DatatableServerSideAsync(request);
        return FromResult(result);
    }
    #endregion
}
