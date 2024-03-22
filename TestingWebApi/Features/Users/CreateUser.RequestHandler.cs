using MediatR;

namespace TestingWebApi.Endpoints.Users
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(
            CreateUserRequest request, 
            CancellationToken cancellationToken)
        {
            return new CreateUserResponse()
            {
                Success = true,
                UserId = Guid.NewGuid().ToString(),
                Email = request.Email
            };
        }
    }
}
