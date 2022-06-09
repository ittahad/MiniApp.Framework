using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MinimalWebApi
{
    public class HealthCheckController : ControllerBase
    {
        private readonly IMediator mediator;

        public HealthCheckController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<string> Ping()
        {
            return await mediator.Send(new HealthCheckQuery());
        }
    }
}
