using System.Text.Json;

namespace EpubWeb.Behaviors
{
    public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        public LogBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
              _logger.LogInformation($" LogBehavior Request -- {JsonSerializer.Serialize(request)}");
            var response = await next();
            _logger.LogInformation($" LogBehavior Response -- {JsonSerializer.Serialize(response)}");
            return response;
        }
    }
}