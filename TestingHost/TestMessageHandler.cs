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
            
            var httpClient = new HttpClient();
            try
            {
                var html = await httpClient.GetStringAsync("http://localhost:5000/TestingWebService/Test/TestPing?q=2");
            }
            catch (Exception ex)
            {

            }
            
            await _mediator.SendToQueue(new TestMessage2 { Name = "Ittahad" }, "TestQueue2");

            var html2 = await httpClient.GetStringAsync("http://localhost:5000/TestingWebService/Test/TestPing?q=3");

            //await _mediator.SendToQueue(new TestMessage2 { Name = "From1 againg" }, "TestQueue2");

            return true;
        }
    }
}
