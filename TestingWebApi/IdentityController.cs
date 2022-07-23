using MediatR;
using Microsoft.AspNetCore.Mvc;
using MinimalFramework;

namespace TestingWebApi
{
    public class IdentityController : ControllerBase
    {
        private IMediator _mediator;

        public IdentityController(IMediator mediator)
        { 
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            bool status = await _mediator.Send(createUserDto);
            return status ? Ok("Success!") : BadRequest("Failed!");
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto createUserDto)
        {
            var response = await _mediator.Send(createUserDto);
            return Ok(response);
        }
    }

    public record CreateUserDto : IRequest<bool>
    { 
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public record AuthenticateUserDto : IRequest<object>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
