using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessage2Handler : MinimalCommandHandler<TestMessage2, bool>
    {
        private readonly ILogger<TestMessage2Handler> _logger;
        private readonly IMinimalMediator _mediator;

        public TestMessage2Handler(
            ILogger<TestMessage2Handler> logger,
            IMinimalMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<bool> Handle(TestMessage2 message)
        {
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("https://www.gmail.com/");
            }
            catch (Exception ex)
            {

            }

            _logger.LogInformation("Message received from Host");

            await _mediator.SendToQueue(new TestMessage3 { Name = "UzZaman" }, "TestQueue2");

            return true;
        }
    }
}
