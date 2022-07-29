using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessageHandler : MinimalCommandHandler<TestMessage, bool>
    {
        private readonly ILogger<TestMessageHandler> _logger;

        public TestMessageHandler(ILogger<TestMessageHandler> logger)
        {
            _logger = logger;
        }

        public override async Task<bool> Handle(TestMessage message)
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


            return true;
        }
    }
}
