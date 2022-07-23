using Microsoft.Extensions.Configuration;
using Moq;
using TestingWebApi;
using Xunit;

namespace UnitTests
{
    public class TokenServiceTests
    {
        [Theory]
        [InlineData("testuser1")]
        [InlineData("testuser2")]
        [InlineData("testuser3")]
        public void Test1(string userName)
        {
            // Arrange
            var confMock = Mock.Of<IConfiguration>();
            var tokenService = new Mock<TokenService>(confMock);
            tokenService.Setup(x => x.GetTokenIssuerInfo())
                .Returns(new TokenIssuerDto()
                {
                    Audience = "*",
                    Issuer = "Minimal Framework",
                    Key = "cad14930-61dd-4176-95d9-864661a88939"
                });

            // Act
            var token = tokenService.Object.BuildToken(userName);

            // Assert
            Assert.NotNull(token);
        }
    }
}