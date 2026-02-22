using Core.Utils.HttpContextManager;
using Microsoft.AspNetCore.Mvc;
using WebUI.Utils.Extensions;

namespace WebUI.Controllers.Base;

public class UIController : BaseController
{
    private readonly IHttpContextManager _httpContextManager;
    public UIController(ILogger<UIController> logger, IHttpContextManager httpContextManager): base(logger) => _httpContextManager = httpContextManager;

    public IActionResult SetCulture(string culture, string? returnUrl)
    {
        _httpContextManager.SetCurrentCulture(culture);
        if (string.IsNullOrWhiteSpace(returnUrl)) return RedirectToAction("Index");
        return Redirect(returnUrl);
    }
}