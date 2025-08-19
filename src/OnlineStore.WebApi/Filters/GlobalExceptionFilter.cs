using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;

namespace OnlineStore.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        
        _logger.LogError(context.Exception, "An unhandled exception occurred. TraceId: {TraceId}", traceId);

        var response = new
        {
            TraceId = traceId,
            Code = "INTERNAL_ERROR",
            Message = "An error occurred while processing your request.",
            Details = context.Exception.Message // In production, you might want to hide this
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
