using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalFramework;

namespace TestingWebApi
{
    public class TestController : ControllerBase
    {
        private readonly IMinimalMediator _minimalMediator;

        public TestController(IMinimalMediator mediator) { 
            _minimalMediator = mediator;
        }

        [HttpGet]
        public async Task<string> TestAction([FromQuery] string q) {
            await _minimalMediator.SendToQueue(new TestMessage { Name = "Akash" }, q);
            await _minimalMediator.SendToQueue(new TestMessage2 { Name = "Ittahad" }, q);
            //await _minimalMediator.SendToExchange(new TestMessage { Name = "Akash" }, "TestExchange2");
            return "Hello";
        }
    }

}
