using MinimalFramework;
using Moq;
using Xunit;

namespace UnitTests
{
    public class TokenServiceTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            TokenService tokenService = new();
            string key = "testskdfhjksdfkjsdhfjksdhfjkshdkfjhsdjkf";
            string issuer = "test";
            string audience = "test";
            string username = "test";

            // Act
            var token = tokenService.BuildToken(key, issuer, audience, username);

            // Assert
            Assert.NotNull(token);
        }
    }
}