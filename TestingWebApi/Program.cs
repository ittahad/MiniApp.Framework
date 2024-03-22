using System.Diagnostics;
using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiniApp.Api;
using MiniApp.Core;
using MiniApp.Redis;
using TestingWebApi;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var options = new MinimalWebAppOptions
{
    UseSwagger = true,
    ConsoleLogging = true,
    FileLogging = true,
    LoggingProvider = LoggingProviders.Serilog,
    UseAuthentication = true,
    SeqLoggerOptions = new()
    {
        UseSeq = false,
        SeqServerUrl = "http://localhost:5341"
    },
    OpenTelemetryOptions = new()
    {
        EnableTracing = false,
        TracingHost = "http://localhost:9411/api/v2/spans"
    }
};

var minimalAppBuilder = new MinimalWebAppBuilder(options);

var minimalWebApp = minimalAppBuilder!.Build(builder =>
{
    builder.AddMediatorAssembly();

    AddAuth(builder);

    builder.Services.AddSingleton<ITokenService, TokenService>();
    builder.Services.AddSingleton<IRedisClient, RedisClient>();
    builder.Services.AddSingleton<HealthCheckQueryHandler>();

    builder!.Services!
        .AddFastEndpoints()
        .AddResponseCaching()
        .SwaggerDocument();
});

minimalWebApp!.Application!
    .UseResponseCaching()
    .UseFastEndpoints()
    .UseSwaggerGen();

minimalWebApp!.Start();

static void AddAuth(WebApplicationBuilder builder)
{
    #region JWT Conf
    var jwtConf = builder.Configuration.GetSection("JwtConfig");
    string issuer = jwtConf["Issuer"];
    string audience = jwtConf["Audience"];
    string key = jwtConf["Key"];

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });
    #endregion
}