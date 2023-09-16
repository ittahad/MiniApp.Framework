using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace TestingWebApi
{
    public class AuthenticateUserRequestHandler : IRequestHandler<AuthenticateUserDto, object>
    {
        private readonly ITokenService _tokenService;

        public AuthenticateUserRequestHandler(ITokenService tokenService)
        { 
            _tokenService = tokenService;
        }

        public async Task<object> Handle(AuthenticateUserDto request, CancellationToken cancellationToken)
        {
            var mongoDb = new MongoClient("mongodb://localhost:27017");
            var collection = mongoDb.GetDatabase("DemoDatabase")
                .GetCollection<User>($"{nameof(User)}s");

            var filter =
                Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(x => x.Email, request.Email),
                    Builders<User>.Filter.Eq(x => x.Password, CreateUserRequestHandler.ComputeSha256Hash(request.Password)));

            var hasDataAlready = (await collection.FindAsync(filter)).Any();

            if (!hasDataAlready) return new
            {
                succes = false,
            };

            var token = _tokenService.BuildToken(request.Email);

            return new { 
                access_token = token,
                succes = true,
                life_time_sec = 30 * 60
            };
        }
    }

    public class User {
        [BsonId]
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }

        public static User MapDto(CreateUserDto dto) => new User() { 
            Email = dto.Email, 
            UserId = Guid.NewGuid().ToString(),
            Name = dto.Name,
            Password = dto.Password,
        };
    }

}
