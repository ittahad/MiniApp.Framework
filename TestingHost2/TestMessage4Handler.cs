using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessage4Handler : MinimalCommandHandler<TestMessage4, bool>
    {
        private readonly ILogger<TestMessage4Handler> _logger;
        private readonly IMinimalMediator _mediator;

        public TestMessage4Handler(
            ILogger<TestMessage4Handler> logger,
            IMinimalMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<bool> Handle(TestMessage4 message)
        {
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("https://yahoo.com/");
            }
            catch (Exception ex)
            {

            }

            _logger.LogInformation("Message received from Host");

            return true;
        }
    }
}
