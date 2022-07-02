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

        public override async Task Handle(TestMessage2 message)
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
            return;
        }
    }
}
