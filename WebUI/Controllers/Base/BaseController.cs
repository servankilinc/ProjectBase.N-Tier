using Core.Enums;
using Core.Utils;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public abstract class BaseController : Controller
{
    private readonly ILogger<BaseController> _logger;
    public BaseController(ILogger<BaseController> logger) => _logger = logger;


    /// <summary>
    /// Handle result and return Json content
    /// </summary>
    protected IActionResult ToJsonResult(Core.Utils.ResultPattern.IResult result)
    {
        if (result.IsSuccess)
            return Ok();

        LogFailedProcess(result);

        var problemDetails = result.GetProblemDetail();
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }


    /// <summary>
    /// Handle result and return error view
    /// </summary>
    protected IActionResult ToErrorView(Result result)
    {
        if (result.IsSuccess)
            return RedirectToAction("Index", "Home");

        LogFailedProcess(result);

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(),
            ErrorType.Forbidden => Forbid(),
            ErrorType.Validation => BadRequest(result.Message),
            ErrorType.Failure => BadRequest(result.Message),
            _ => StatusCode(500)
        };
    }


    protected void AddValidationFailuresToModel(Core.Utils.ResultPattern.IResult result)
    {
        if (result.Error?.Type == ErrorType.Validation && result.Error.ValidationFailures != null)
        {
            foreach (var failure in result.Error.ValidationFailures)
                foreach (var errorMessage in failure.Value)
                    ModelState.AddModelError(failure.Key, errorMessage);
        }
    }


    private void LogFailedProcess(Core.Utils.ResultPattern.IResult result)
    {
        if (result.Error == null) return;

        switch (result.Error.Type)
        {
            case ErrorType.Failure:
                _logger.LogError("Failure process: \nMessage: {Message} \nDetail: {@Error}",
                    result.Message,
                    result.Error
                );
                break;
            case ErrorType.NotFound:
                _logger.LogWarning("Not found: \nMessage: {Message} \nDetail: {@Error}",
                    result.Message,
                    result.Error
                );
                break;
            case ErrorType.Validation:
                _logger.LogInformation("Validation error: \nMessage: {Message} \nDetail: {@Error}",
                    result.Message,
                    result.Error
                );
                break;
            case ErrorType.Forbidden:
                _logger.LogWarning("Forbidden : \nMessage: {Message} \nDetail: {@Error}",
                    result.Message,
                    result.Error
                );
                break;
            default:
                _logger.LogError("Unknown error: \nMessage: {Message} \nDetail: {@Error}",
                    result.Message,
                    result.Error
                );
                break;
        }
    }
}
