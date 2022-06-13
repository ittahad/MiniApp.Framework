using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MinimalWebApi
{
    public class HealthCheckController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<HealthCheckController> logger;

        public HealthCheckController(
            IMediator mediator, 
            ILogger<HealthCheckController> logger) {

            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<string> Ping()
        {
            this.logger.LogInformation("Health check.....");

            return await mediator.Send(new HealthCheckQuery());
        }
    }
}
