using AutoMapper;
using Core.Utils.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using WebUI.Models.Auth;

namespace WebUI.Controllers;

[AllowAnonymous]
public class AccountController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILoggingService _loggingService;
    private readonly IMapper _mapper;
    public AccountController(ILogger<AccountController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, ILoggingService loggingService) : base(logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _loggingService = loggingService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var model = new LoginRequest();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return View(loginRequest);

        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            _loggingService.LogWarning($"Email or password is incorrect {loginRequest.Email}");
            ModelState.AddModelError("", "Email or password is incorrect");
            return View(loginRequest);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, isPersistent: loginRequest.RememberMe, lockoutOnFailure: true);
        if (result.IsLockedOut)
        {
            _loggingService.LogWarning($"User account is locked, email address: {loginRequest.Email}");
            ModelState.AddModelError("", "Your account is locked.");
            return View(loginRequest);
        }
        else if (!result.Succeeded)
        {
            _loggingService.LogWarning($"Email or password is incorrect {loginRequest.Email}");
            ModelState.AddModelError("", "Email or password is incorrect");
            return View(loginRequest);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
    {
        if (!ModelState.IsValid)
            return View(signUpRequest);

        var userExist = await _userManager.FindByEmailAsync(signUpRequest.Email);
        if (userExist != null)
        {
            _loggingService.LogWarning($"Email is already in use {signUpRequest.Email}");
            ModelState.AddModelError("", "Email is already in use");
            return View(signUpRequest);
        }

        var user = _mapper.Map<User>(signUpRequest);
        user.UserName = Guid.NewGuid().ToString();

        var result = await _userManager.CreateAsync(user, signUpRequest.Password);
        if (!result.Succeeded)
        {
            _loggingService.LogWarning($"User cannot be created {signUpRequest.Email} error list: {string.Join("\n", result.Errors.Select(e => e.Description))}");
            ModelState.AddModelError("", "User cannot be created");
            return View(signUpRequest);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        var resultSignIn = await _signInManager.PasswordSignInAsync(user, signUpRequest.Password, isPersistent: true, lockoutOnFailure: false);
        if (resultSignIn.IsLockedOut)
        {
            _loggingService.LogWarning($"User account is locked {signUpRequest.Email}");
            ModelState.AddModelError("", "User account is locked");
            return View(signUpRequest);
        }
        else if (!resultSignIn.Succeeded)
        {
            _loggingService.LogWarning($"User created but an error occured on signup process {signUpRequest.Email}");
            ModelState.AddModelError("", "User created successfully.");
            return View(signUpRequest);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
