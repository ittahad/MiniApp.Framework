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

        [Fact]
        public void Test2()
        {
            // Arrange
            TokenService tokenService = new();
            string key = "42342342342324524wrertertertertert";
            string issuer = "testfdg";
            string audience = "testdfg";
            string username = "testdfgs";

            // Act
            var token = tokenService.BuildToken(key, issuer, audience, username);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public void Test3()
        {
            // Arrange
            TokenService tokenService = new();
            string key = "sdfjkshdfjshdfkjshdjkfhsdf";
            string issuer = "sdfsdf";
            string audience = "tessdftdfg";
            string username = "sdfsdf";

            // Act
            var token = tokenService.BuildToken(key, issuer, audience, username);

            // Assert
            Assert.NotNull(token);
        }
    }
}