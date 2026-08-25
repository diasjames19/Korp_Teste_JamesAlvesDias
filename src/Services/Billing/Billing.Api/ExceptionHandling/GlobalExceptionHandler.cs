using Billing.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Api.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception switch
{
    ArgumentException =>
        StatusCodes.Status400BadRequest,

    KeyNotFoundException =>
        StatusCodes.Status404NotFound,

    InsufficientStockException =>
        StatusCodes.Status409Conflict,

    InvalidOperationException =>
        StatusCodes.Status409Conflict,

    _ =>
        StatusCodes.Status500InternalServerError
};

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "An unexpected error occurred.");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "A business validation error occurred.");
        }

        var detail = statusCode ==
                     StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred."
            : exception.Message;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
    {
        StatusCodes.Status400BadRequest =>
            "Bad Request",

        StatusCodes.Status404NotFound =>
            "Not Found",

        StatusCodes.Status409Conflict =>
            "Conflict",

        _ =>
            "Internal Server Error"
    };
    }
}