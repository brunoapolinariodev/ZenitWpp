using MediatR;
using Microsoft.Extensions.Logging;

namespace ZenitWpp.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;

        _logger.LogInformation("[START] {Request}", name);
        var start = DateTime.UtcNow;

        var response = await next();

        var elapsed = (DateTime.UtcNow - start).TotalMilliseconds;
        _logger.LogInformation("[END] {Request} ({ElapsedMs}ms)", name, elapsed);

        return response;
    }
}
