using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.PipelineBehaviors
{
    public class PerformanceMonitoringBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>

    {
        private readonly ILogger _logger;

        public PerformanceMonitoringBehavior(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("PerformanceMonitoringBehavior");
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation($"Starting processing for {typeof(TRequest).Name}");

            var response = await next(); // Call the next delegate in the pipeline

            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation($"Finished processing {typeof(TRequest).Name} in {elapsedTime} ms");

            return response;
        }
    }
}
