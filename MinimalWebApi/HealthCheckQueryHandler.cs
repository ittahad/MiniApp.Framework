using MediatR;
using Microsoft.Extensions.Configuration;

namespace MinimalWebApi
{
    public class HealthCheckQueryHandler : IRequestHandler<HealthCheckQuery, string>
    {
        private readonly IConfiguration _configuration;

        public HealthCheckQueryHandler(IConfiguration configuration) { 
            _configuration = configuration;
        }

        public async Task<string> Handle(HealthCheckQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult($"Service is running...");
        }
    }
}
