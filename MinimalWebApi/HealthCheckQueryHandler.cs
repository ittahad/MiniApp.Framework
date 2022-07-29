using MediatR;
using Microsoft.Extensions.Configuration;
using MinimalFramework;

namespace MinimalWebApi
{
    public class HealthCheckQueryHandler 
        : MinimalQueryHandler<HealthCheckQuery, string>
    {
        private readonly IConfiguration _configuration;

        public HealthCheckQueryHandler(IConfiguration configuration) { 
            _configuration = configuration;
        }

        public override async Task<string> Handle(
            HealthCheckQuery request, 
            CancellationToken cancellationToken)
        {
            return await Task.FromResult($"Service is running...");
        }
    }
}
