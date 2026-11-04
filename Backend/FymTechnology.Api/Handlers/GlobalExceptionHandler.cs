using FymTechnology.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FymTechnology.Api.Handlers;

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
        var (statusCode, title) = exception switch
        {
            NotFoundException =>
                (StatusCodes.Status404NotFound, "Recurso no encontrado."),

            ConflictException =>
                (StatusCodes.Status409Conflict, "Conflicto de datos."),

            BusinessRuleException =>
                (StatusCodes.Status400BadRequest, "Regla de negocio."),

            UnauthorizedAccessException =>
                (StatusCodes.Status401Unauthorized, "No autorizado."),

            _ =>
                (StatusCodes.Status500InternalServerError, "Error interno del servidor.")
        };

        if (statusCode >= 500)
        {
            _logger.LogError(exception, "Unhandled exception.");
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode >= 500
                ? "Ocurrió un error inesperado."
                : exception.Message
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}