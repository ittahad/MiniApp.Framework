using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestingWebApi;

namespace TestingWebApi;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string BuildToken(string userName)
    {
        var info = GetTokenIssuerInfo();

        TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(info.Key));

        var credentials = new SigningCredentials(
            key: securityKey,
            algorithm: SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: info.Issuer,
            audience: info.Audience,
            claims: claims,
            expires: DateTime.Now.Add(ExpiryDuration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(tokenDescriptor);
    }

    public virtual TokenIssuerDto GetTokenIssuerInfo()
    {
        var jwtConf = _configuration.GetSection("JwtConfig");
        string issuer = jwtConf["Issuer"];
        string audience = jwtConf["Audience"];
        string key = jwtConf["Key"];

        return new TokenIssuerDto() { 
            Key = key,
            Audience = audience,
            Issuer = issuer,
        };
    }
}

public class TokenIssuerDto
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}