using Azure;
using Core.Enums;
using Core.Utils;
using Core.Utils.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public abstract class BaseController : Controller
{
    private readonly ILogger<BaseController> _logger;
    public BaseController(ILogger<BaseController> logger) => _logger = logger;


    protected IActionResult ApiResult(Result result)
    {
        if (result.IsSuccess)
            return Ok();

        LogFailedProcess(result);

        if (result.Error.ValidationFailures != null)
        {
            foreach (var failure in result.Error.ValidationFailures)
                foreach (var errorMessage in failure.Value)
                    ModelState.AddModelError(failure.Key, errorMessage);
        }

        var problemDetails = result.GetProblemDetail();
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected IActionResult ApiResult<TData>(Result<TData> result)
    {
        if (result.IsSuccess)
            return Ok(result.Data);

        LogFailedProcess(result);

        if (result.Error.ValidationFailures != null)
        {
            foreach (var failure in result.Error.ValidationFailures)
                foreach (var errorMessage in failure.Value)
                    ModelState.AddModelError(failure.Key, errorMessage);
        }

        var problemDetails = result.GetProblemDetail();
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected IActionResult ErrorView(Result result)
    {
        if (result.IsSuccess)
            return RedirectToAction("Index", "Home");

        LogFailedProcess(result);

        // ValidationRuleException => response.Redirect("/Error/InvalidProcess")
        // BusinessException => response.Redirect("/Error/InvalidProcess")
        // GeneralException => response.Redirect("/Error/InternalServer")
        // DataAccessException => response.Redirect("/Error/InternalServer")
        // 404 => response.Redirect("/Error/NotFound")
        // others => response.Redirect("/Error/InternalServer")
        return result.Error.Type switch
        {
            ErrorType.Failure => RedirectToAction("InvalidProcess", "Error"),
            ErrorType.NotFound => RedirectToAction("NotFound", "Error"),
            ErrorType.Validation => RedirectToAction("InvalidProcess", "Error"),
            ErrorType.Forbidden => RedirectToAction("Forbidden", "Error"),
            _ => RedirectToAction("InternalServer", "Error"),
        };
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
