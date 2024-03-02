using Microsoft.Extensions.Logging;
using MiniApp.Core;

namespace TestingHost
{
    public class TestMessage2Handler : MinimalCommandHandler<TestMessage2, bool>
    {
        private readonly ILogger<TestMessage2Handler> _logger;
        private readonly IMinimalMediator _mediator;
        private readonly IMinimalHttpClient _minimalHttpClient;

        public TestMessage2Handler(
            ILogger<TestMessage2Handler> logger,
            IMinimalMediator mediator,
            IMinimalHttpClient minimalHttpClient)
        {
            _logger = logger;
            _mediator = mediator;
            _minimalHttpClient = minimalHttpClient;
        }

        public override async Task<bool> Handle(TestMessage2 message)
        {
            try
            {
                using var httpRquestMessage = new HttpRequestMessage();
                httpRquestMessage.RequestUri = new Uri("http://localhost:5000/TestingWebService/Test2/TestPing?q=4");
                httpRquestMessage.Method = HttpMethod.Get;
                var data = await _minimalHttpClient.MakeHttpRequest<string>(httpRquestMessage);
            }
            catch (Exception)
            {
                _logger.LogInformation("ERROR");
            }

            _logger.LogInformation("Message received from Host");

            await _mediator.SendAsync(new TestMessage3 { Name = "UzZaman" }, "TestQueue2");

            return true;
        }
    }
}
