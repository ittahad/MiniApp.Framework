

namespace TestingWebApi.Endpoints.Users
{
    public class CreateUserResponse
    {
        public bool Success { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}
