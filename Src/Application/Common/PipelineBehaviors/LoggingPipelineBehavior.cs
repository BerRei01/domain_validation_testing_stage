using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.PipelineBehaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingPipelineBehavior(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("LoggingPipelineBehavior");;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"LoggingPipelineBehaviour: Handling {typeof(TRequest).Name} with data: {request}");

            var response = await next(); // Call the next delegate in the pipeline

            _logger.LogInformation($"LoggingPipelineBehaviour: Handled {typeof(TRequest).Name} with response: {response}");

            return response;
        }
    }
}
