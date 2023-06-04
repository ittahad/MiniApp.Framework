using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;

namespace TestingHost
{
    public class TestMessage3Handler : MinimalCommandHandler<TestMessage3, bool>
    {
        private readonly ILogger<TestMessage3Handler> _logger;
        private readonly IMinimalMediator _mediator;

        public TestMessage3Handler(ILogger<TestMessage3Handler> logger, IMinimalMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<bool> Handle(TestMessage3 message)
        {
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("http://localhost:5000/TestingWebService/Test/TestPing?q=5");
            }
            catch (Exception ex)
            {

            }
            
            await _mediator.SendToQueue(new TestMessage4 { Name = "UzZaman" }, "TestQueue3");

            _logger.LogInformation("Message received from Host");
            
            return true;
        }
    }
}
