using AutoMapper;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using WebUI.Models.Auth;
using WebUI.Utils.ActionFilters;

namespace WebUI.Controllers;

public class AccountController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, ILogger<AccountController> logger) : base(logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var model = new LoginRequest();
        return View(model);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<LoginRequest>))]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null) return FromResult(Result.Failure($"The email address is not exist {loginRequest.Email} to login", message: "The email address or password was wrong."));

        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.IsLockedOut)
        {
            return FromResult(Result.Failure($"User account is locked, email address: {loginRequest.Email}", message: "Your account is locked."));
        }
        else if (!result.Succeeded)
        {
            return FromResult(Result.Failure($"Invalid login information's, email address: {loginRequest.Email}", message: "The email address or password was wrong."));
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<SignUpRequest>))]
    public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
    {
        var userExist = await _userManager.FindByEmailAsync(signUpRequest.Email);
        if (userExist != null) return FromResult(Result.Failure($"The email address is already in use, email address: {signUpRequest.Email}", message: "The email address is already in use."));

        var user = _mapper.Map<User>(signUpRequest);
        user.UserName = Guid.NewGuid().ToString();

        var result = await _userManager.CreateAsync(user, signUpRequest.Password);
        if (!result.Succeeded) return FromResult(Result.Failure($"User cannot be created. email address: {signUpRequest.Email}, error list: " + string.Join("\n", result.Errors.Select(e => e.Description))));

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded) return FromResult(Result.Failure($"Failed to assign role, email address: {signUpRequest.Email}"));

        var resultSignIn = await _signInManager.PasswordSignInAsync(user, signUpRequest.Password, isPersistent: true, lockoutOnFailure: false);
        if (resultSignIn.IsLockedOut)
        {
            return FromResult(Result.Failure($"User account is locked, email address: {signUpRequest.Email}", message: "Your account is locked."));
        }
        else if (!resultSignIn.Succeeded)
        {
            return FromResult(Result.Failure($"An error occured on signup process, email address: {signUpRequest.Email}"));
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
