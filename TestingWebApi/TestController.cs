using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestingWebApi
{
    [Authorize]
    public class TestController : ControllerBase
    {
        public string TestAction() {            
            return "Hello";
        }
    }
}
