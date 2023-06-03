using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessageHandler : MinimalCommandHandler<TestMessage, bool>
    {
        private readonly ILogger<TestMessageHandler> _logger;
        private readonly IMinimalMediator _mediator;

        public TestMessageHandler(
            ILogger<TestMessageHandler> logger,
            IMinimalMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
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
            
            await _mediator.SendToQueue(new TestMessage2 { Name = "Ittahad" }, "TestQueue2");

            return true;
        }
    }
}
