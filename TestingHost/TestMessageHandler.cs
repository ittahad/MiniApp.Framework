using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessageHandler : MinimalMessageHandler<TestMessage>
    {
        private readonly ILogger<TestMessageHandler> _logger;

        public TestMessageHandler(ILogger<TestMessageHandler> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(TestMessage message)
        {
            _logger.LogInformation("Message received from Host");
            
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("https://www.facebook.com/");
            }
            catch (Exception ex)
            {

            }


            return;
        }
    }
}
