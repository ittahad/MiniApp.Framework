using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessage3Handler : MinimalCommandHandler<TestMessage3, bool>
    {
        private readonly ILogger<TestMessage3Handler> _logger;

        public TestMessage3Handler(ILogger<TestMessage3Handler> logger)
        {
            _logger = logger;
        }

        public override async Task<bool> Handle(TestMessage3 message)
        {
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("https://www.youtube.com");
            }
            catch (Exception ex)
            {

            }

            _logger.LogInformation("Message received from Host");
            
            return true;
        }
    }
}
