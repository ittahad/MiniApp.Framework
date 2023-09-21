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

    public TokenService(
        IConfiguration configuration,
        IAppTenantContext<ApplicationTenantPgSql> appTenantContext,
        IHttpContextAccessor httpContextAccessor,
        IAppDbContext appDbContext)
    {
        _configuration = configuration;
        _appTenantContext = appTenantContext;
        _httpContextAccessor = httpContextAccessor;
        _appDbContext = appDbContext;
    }

    public string BuildToken(string userName)
    {
        var info = GetTokenIssuerInfo();

        TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

        var origin = _httpContextAccessor.HttpContext?.Request.Headers["Origin"];

        var tenantInfo = _appTenantContext.GetApplicationTenant(origin!);

        // Testing
        string tenant = "3A03CB43-7406-4DB3-B230-EA998A732306";
        //_ = _appDbContext.SaveItem(new Student() { Id = "234234346768", Name = Guid.NewGuid().ToString().Substring(0, 10) }, tenant).Result;
        //var student = _appDbContext.GetItem<Student>(x => x.Id == "23423434", tenant).Result;
        //_ = _appDbContext.Update<Student>(x => x.Id == "23423434", student, tenant).Result;
        //_ = _appDbContext.DeleteItem<Student>(x => x.Id == "asd", tenant).Result;

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