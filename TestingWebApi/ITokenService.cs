namespace TestingWebApi;

public interface ITokenService
{
    public string BuildToken(string userName);
    public TokenIssuerDto GetTokenIssuerInfo();
}

