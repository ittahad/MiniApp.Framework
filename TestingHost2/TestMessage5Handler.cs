using Microsoft.Extensions.Logging;
using MiniApp.Core;

namespace TestingHost
{
    public class TestMessage5Handler : MinimalCommandHandler<TestMessage5, bool>
    {
        private readonly ILogger<TestMessage5Handler> _logger;

        public TestMessage5Handler(
            ILogger<TestMessage5Handler> logger)
        {
            _logger = logger;
        }

        public override async Task<bool> Handle(TestMessage5 message)
        {
            _logger.LogInformation("Message received from Host");

            return true;
        }
    }
}
