using Microsoft.IdentityModel.Tokens;
using MiniApp.Core;
using MiniApp.MongoDB;
using MiniApp.PgSQL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestingWebApi;

public class Student
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IAppTenantContext<ApplicationTenantPgSql> _appTenantContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAppDbContext _appDbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITenantDataContextResolver _tenantDataContextResolver;
    private readonly AppTenantContextInfo _contextInfo;

    public TokenService(
        IConfiguration configuration,
        IAppTenantContext<ApplicationTenantPgSql> appTenantContext,
        IHttpContextAccessor httpContextAccessor,
        IAppDbContext appDbContext,
        IServiceProvider serviceProvider,
        ITenantDataContextResolver tenantDataContextResolver,
        AppTenantContextInfo contextInfo)
    {
        _configuration = configuration;
        _appTenantContext = appTenantContext;
        _httpContextAccessor = httpContextAccessor;
        _appDbContext = appDbContext;
        _serviceProvider = serviceProvider;
        _contextInfo = contextInfo;
        _tenantDataContextResolver = tenantDataContextResolver;
    }

    public string BuildToken(string userName)
    {
        var info = GetTokenIssuerInfo();

        TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

        var origin = _httpContextAccessor.HttpContext?.Request.Headers["Origin"];
        
        var tenantInfo = _appTenantContext.GetApplicationTenant(origin!);

        
        var tenants = _tenantDataContextResolver.GetAllTenants(_configuration["DbConnectionString"]);
        var tenant1 = _tenantDataContextResolver.GetApplicationTenant(origin);
        var tenant2 = _tenantDataContextResolver.GetApplicationTenantById("3A03CB43-7406-4DB3-B230-EA998A732306");

        var str = tenant2.ItemId.ToString();

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