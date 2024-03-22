using MiniApp.Core;

namespace TestingWebApi.Endpoints.Users
{
    public class CreateUserRequest : MinimalQuery<CreateUserResponse>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
