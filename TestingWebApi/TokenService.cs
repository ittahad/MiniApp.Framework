using Microsoft.IdentityModel.Tokens;
using MiniApp.Core;
using MiniApp.MongoDB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestingWebApi;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IAppTenantContext<ApplicationTenantMongoDb> _appTenantContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(
        IConfiguration configuration,
        IAppTenantContext<ApplicationTenantMongoDb> appTenantContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _appTenantContext = appTenantContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public string BuildToken(string userName)
    {
        var info = GetTokenIssuerInfo();

        TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

        var origin = _httpContextAccessor.HttpContext?.Request.Headers["Origin"];

        var tenantInfo = _appTenantContext.GetApplicationTenant(origin!);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(TokenClaims.TenantIdClaim, tenantInfo.TenantId)
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

public static class TokenClaims
{
    public static string TenantIdClaim { get; }  = "TenantId";
}