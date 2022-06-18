﻿using Microsoft.AspNetCore.Authorization;
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

        public async Task<string> TestAction() {
            await _minimalMediator.SendToQueue(new TestMessage { Name = "Akash" }, "TestQueue1");
            //await _minimalMediator.SendToExchange(new TestMessage { Name = "Akash" }, "TestExchange2");
            return "Hello";
        }
    }

}
