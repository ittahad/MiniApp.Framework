using FastEndpoints;
using MiniApp.Core;

namespace TestingWebApi.Endpoints.Users
{
    public class CreateUser : Endpoint<CreateUserRequest, CreateUserResponse>
    {
        private readonly IMinimalMediator _mediator;

        public CreateUser(IMinimalMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/users/create");
            Throttle(
                hitLimit: 3,
                durationSeconds: 10
            );
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateUserRequest request, CancellationToken ct)
        {
            var response = await _mediator.SendAsync<CreateUserRequest, CreateUserResponse>(request);

            await SendAsync(response, cancellation: ct);
        }
    }
}
