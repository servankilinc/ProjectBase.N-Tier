using API.Controllers.Base;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Model.Auth.Login;
using Model.Auth.Refresh;
using Model.Auth.SignUp;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseController
{
    private readonly IAuthService _authService;
    public AccountController(IAuthService authService, ILogger<AccountController> logger) : base(logger) => _authService = authService;


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return ToAction(result);
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var result = await _authService.SignUpAsync(request);

        return ToAction(result);
    }

    [HttpPost("RefreshAuth")]
    public async Task<IActionResult> RefreshAuth(RefreshAuthRequest request)
    {
        var result = await _authService.RefreshAuthAsync(request);

        return ToAction(result);
    }
}