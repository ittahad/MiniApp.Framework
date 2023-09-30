using Microsoft.Extensions.Logging;
using MiniApp.Core;

namespace TestingHost
{
    public class TestMessage5Handler : MinimalCommandHandler<TestMessage5, bool>
    {
        private readonly ILogger<TestMessage5Handler> _logger;
        //private readonly IMinimalMediator _mediator;
        //private readonly IMinimalHttpClient _minimalHttpClient;

        public TestMessage5Handler(
            ILogger<TestMessage5Handler> logger
            //IMinimalMediator mediator,
            /*IMinimalHttpClient minimalHttpClient*/)
        {
            _logger = logger;
            //_mediator = mediator;
            //_minimalHttpClient = minimalHttpClient;
        }

        public override async Task<bool> Handle(TestMessage5 message)
        {
            try
            {
                using var httpRquestMessage = new HttpRequestMessage();
                /*httpRquestMessage.RequestUri = new Uri("http://localhost:5000/TestingWebService/Test/TestPing?q=abc");
                httpRquestMessage.Method = HttpMethod.Get;
                var data = await _minimalHttpClient.MakeHttpRequest<string>(httpRquestMessage);*/
            }
            catch (Exception ex)
            {

            }

            _logger.LogInformation("Message received from Host");

            return true;
        }
    }
}
