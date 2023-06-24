using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalFramework;
using MinimalHttpClient;
using MinimalRedis;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace TestingWebApi
{
    public class TestController : ControllerBase
    {
        private readonly IMinimalMediator _minimalMediator;
        private readonly IConfiguration _configuration;
        private readonly IMinimalHttpClient _minimalHttpClient;
        private readonly ILogger<TestController> _logger;
        private readonly ActivitySource _activitySource;
        private readonly RedisClient _redisClient;
       // private readonly Counter<int> _requestCounter;

        public TestController(
            IMinimalMediator mediator,
            IConfiguration configuration,
            IMinimalHttpClient minimalHttpClient,
            ILogger<TestController> logger,
            ActivitySource activitySource,
            RedisClient client
            /*Meter meter*/) { 
            _minimalMediator = mediator;
            _configuration = configuration;
            _activitySource = activitySource;
            _logger = logger;
            _minimalHttpClient = minimalHttpClient;
            _redisClient = client;
            //_requestCounter = meter.CreateCounter<int>("compute_requests");
        }

        [HttpGet]
        //[Authorize]
        public async Task<string> TestAction([FromQuery] string q) {

            //_requestCounter.Add(1);

            /*using var activity = _activitySource.StartActivity("SomeActivity");

            activity?.SetBaggage("Name", "Akash");

            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, World!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });*/
            
            _redisClient.Subscribe("IttahadAkash", (ch, msg) => { });
            _redisClient.Publish("IttahadAkash", "test");

            try
            {
                //throw new Exception();
                _logger.LogInformation($"+++++++++ TraceId: {Activity.Current.TraceId} +++++++++");

                using var httpRquestMessage = new HttpRequestMessage();
                httpRquestMessage.RequestUri = new Uri("http://localhost:5000/TestingWebService/Test/TestPing?q=1");
                httpRquestMessage.Method = HttpMethod.Get;
                var data = await _minimalHttpClient.MakeHttpRequest<string>(httpRquestMessage);
            }
            catch (Exception ex) { 
            
            }

            try { 
                throw new Exception();
            }
            catch(Exception ex)
            {

            }

            try { 
                throw new Exception();
            }
            catch(Exception ex)
            {

            }

            await _minimalMediator.SendAsync(new TestMessage { Name = "Akash" }, q);

            //await _minimalMediator.SendToQueue(new TestMessage2 { Name = "Ittahad" }, q);

            //await _minimalMediator.SendToExchange(new TestMessage { Name = "Akash" }, "TestExchange2");


            return "Hello";
        }

        [HttpGet]
        [Authorize]
        public IActionResult SomeAction([FromQuery] string q)
        {
            return Ok("Done");
        }

        [HttpGet]
        public IActionResult TestPing([FromQuery] string q)
        {
            return Ok("Success=" + q);
        }
    }

}
