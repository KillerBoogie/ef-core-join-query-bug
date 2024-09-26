using MediatR;
using Serilog;
using System.Diagnostics;
using System.Text.Json;

namespace VC.WebApi.Infrastructure.MediatR.Piplines;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger logger, JsonSerializerOptions jsonSerializerOptions) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        Stopwatch stopwatch = new();
        // Log the request content
        logger.Information($"Handling {typeof(TRequest).Name} with content: {@request}");

        stopwatch.Start();

        var response = await next();

        stopwatch.Stop();

        // Log the response content
        logger.Information(
            $"Handled {typeof(TRequest).Name} in {stopwatch.ElapsedMilliseconds} ms with response: {JsonSerializer.Serialize(response, jsonSerializerOptions)}"
        );

        stopwatch.Reset();

        return response;
    }
}