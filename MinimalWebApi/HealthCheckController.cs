using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace MinimalWebApi
{
    public class HealthCheckController : ControllerBase
    {
        private readonly IMinimalMediator mediator;
        private readonly ILogger<HealthCheckController> logger;

        public HealthCheckController(
            IMinimalMediator mediator,
            ILogger<HealthCheckController> logger) {

            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<string> Ping()
        {
            this.logger.LogInformation("Health check.....");

            return await mediator.SendAsync<HealthCheckQuery, string>(new HealthCheckQuery());
        }
    }
}
