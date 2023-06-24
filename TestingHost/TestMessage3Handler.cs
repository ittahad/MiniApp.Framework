using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalFramework;
using MinimalHttpClient;

namespace TestingHost
{
    public class TestMessage3Handler : MinimalCommandHandler<TestMessage3, bool>
    {
        private readonly ILogger<TestMessage3Handler> _logger;
        private readonly IMinimalMediator _mediator;
        private readonly IMinimalHttpClient _minimalHttpClient;

        public TestMessage3Handler(
            ILogger<TestMessage3Handler> logger, 
            IMinimalMediator mediator,
            IMinimalHttpClient minimalHttpClient)
        {
            _logger = logger;
            _mediator = mediator;
            _minimalHttpClient = minimalHttpClient;
        }

        public override async Task<bool> Handle(TestMessage3 message)
        {
            try
            {
                using var httpRquestMessage = new HttpRequestMessage();
                httpRquestMessage.RequestUri = new Uri("http://localhost:5000/TestingWebService/Test/TestPing?q=5");
                httpRquestMessage.Method = HttpMethod.Get;
                var data = await _minimalHttpClient.MakeHttpRequest<string>(httpRquestMessage);
            }
            catch (Exception ex)
            {

            }
            
            await _mediator.SendAsync(new TestMessage4 { Name = "UzZaman" }, "TestQueue3");

            _logger.LogInformation("Message received from Host");
            
            return true;
        }
    }
}
