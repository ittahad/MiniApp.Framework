using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessage2Handler : MinimalMessageHandler<TestMessage2>
    {
        private readonly ILogger<TestMessage2Handler> _logger;

        public TestMessage2Handler(ILogger<TestMessage2Handler> logger)
        {
            _logger = logger;
        }

        public override Task Handle(TestMessage2 message)
        {
            _logger.LogInformation("Message received from Host");
            return Task.CompletedTask;
        }
    }
}
