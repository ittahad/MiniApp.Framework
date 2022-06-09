using Microsoft.IdentityModel.Tokens;
using MinimalFramework;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    public string BuildToken(string key,
        string issuer,
        string audience,
        string userName) {
        
        TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        
        var credentials = new SigningCredentials(
            key: securityKey,
            algorithm: SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.Add(ExpiryDuration), 
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(tokenDescriptor);
    }
}