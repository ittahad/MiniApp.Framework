using MediatR;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace TestingWebApi
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserDto, bool>
    {
        public async Task<bool> Handle(CreateUserDto request, CancellationToken cancellationToken)
        {
            var mongoDb = new MongoClient("mongodb://localhost:27017");
            var collection = mongoDb.GetDatabase("DemoDatabase")
                .GetCollection<User>($"{nameof(User)}s");

            var filter = Builders<User>.Filter.Eq(x => x.Email, request.Email);
            var hasDataAlready = (await collection.FindAsync(filter)).Any();

            if (hasDataAlready) return false;

            var modifiedUserWithHashedPass = request with { 
                Password = ComputeSha256Hash(request.Password),
            };

            collection.InsertOne(User.MapDto(modifiedUserWithHashedPass));
            return true;
        }

        public static string ComputeSha256Hash(string? rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            { 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
 
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}
