using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;

namespace TestingHost
{
    public class TestMessageHandler : MinimalCommandHandler<TestMessage, bool>
    {
        private readonly ILogger<TestMessageHandler> _logger;
        private readonly IMinimalMediator _mediator;
        private readonly IMinimalHttpClient _minimalHttpClient;

        public TestMessageHandler(
            ILogger<TestMessageHandler> logger,
            IMinimalMediator mediator,
            IMinimalHttpClient minimalHttpClient)
        {
            _logger = logger;
            _mediator = mediator;
            _minimalHttpClient = minimalHttpClient;
        }

        public override async Task<bool> Handle(TestMessage message)
        {
            _logger.LogInformation("Message received from Host");
            
            var httpClient = new HttpClient();
            try
            {
                
                using var httpRquestMessage = new HttpRequestMessage();
                httpRquestMessage.RequestUri = new Uri("http://localhost:5000/TestingWebService/Test/TestPing?q=2");
                httpRquestMessage.Method = HttpMethod.Get;
                var data = await _minimalHttpClient.MakeHttpRequest<string>(httpRquestMessage);
            }
            catch (Exception ex)
            {

            }
            
            await _mediator.SendAsync(new TestMessage2 { Name = "Ittahad" }, "TestQueue2");

            var html2 = await httpClient.GetStringAsync("http://localhost:5000/TestingWebService/Test/TestPing?q=3");

            //await _mediator.SendToQueue(new TestMessage2 { Name = "From1 againg" }, "TestQueue2");

            return true;
        }
    }
}
