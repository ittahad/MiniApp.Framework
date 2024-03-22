using FastEndpoints;

namespace TestingWebApi.Features.Users
{
    public class GetUser : Endpoint<GetUserRequest, GetUserResponse>
    {
        public override void Configure()
        {
            Get("/users/get");
            ResponseCache(30);
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetUserRequest request, CancellationToken ct)
        {
            var response = new GetUserResponse()
            {
                UserId = request.UserId,
                Time = DateTime.UtcNow
            };
            await SendOkAsync(response, cancellation: ct);
        }
    }
}
