using System.Net;
using System.Text.Json;
using FluentValidation;

namespace ZenitWpp.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message, errors) = ex switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Erro de validação.",
                ve.Errors.Select(e => e.ErrorMessage)
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                ex.Message,
                Enumerable.Empty<string>()
            ),
            InvalidOperationException => (
                HttpStatusCode.Conflict,
                ex.Message,
                Enumerable.Empty<string>()
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Não autorizado.",
                Enumerable.Empty<string>()
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "Erro interno do servidor.",
                Enumerable.Empty<string>()
            )
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors = errors.ToList()
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
