using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalFramework;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace TestingWebApi
{
    public class TestController : ControllerBase
    {
        private readonly IMinimalMediator _minimalMediator;
        private readonly IConfiguration _configuration;
        private readonly ActivitySource _activitySource;
       // private readonly Counter<int> _requestCounter;

        public TestController(
            IMinimalMediator mediator,
            IConfiguration configuration,
            ActivitySource activitySource
            /*Meter meter*/) { 
            _minimalMediator = mediator;
            _configuration = configuration;
            _activitySource = activitySource;
            //_requestCounter = meter.CreateCounter<int>("compute_requests");
        }

        [HttpGet]
        public async Task<string> TestAction([FromQuery] string q) {

            //_requestCounter.Add(1);

            using var activity = _activitySource.StartActivity("SomeActivity");

            activity?.SetBaggage("Name", "Akash");

            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, World!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });

            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync("https://www.google.com/");
            }
            catch (Exception ex) { 
            
            }

            await _minimalMediator.SendToQueue(new TestMessage { Name = "Akash" }, q);
            await _minimalMediator.SendToQueue(new TestMessage2 { Name = "Ittahad" }, q);

            //await _minimalMediator.SendToExchange(new TestMessage { Name = "Akash" }, "TestExchange2");


            return "Hello";
        }
    }

}
