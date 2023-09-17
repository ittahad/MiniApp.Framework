using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiniApp.Api;
using MiniApp.Core;
using MiniApp.MongoDB;
using MiniApp.PgSQL;
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
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    },
    OpenTelemetryOptions = new()
    {
        EnableTracing = true,
        TracingHost = "http://localhost:9411/api/v2/spans"
    }
};

var minimalAppBuilder = new MinimalWebAppBuilder(options);

var minimalWebApp = minimalAppBuilder?.Build(builder =>
{
    builder.AddMediatorAssembly();

    builder.Services.AddMongoDB();
    //builder.Services.AddPgSql();

    builder.Services.AddSingleton<ITokenService, TokenService>();
    builder.Services.AddSingleton<IRedisClient, RedisClient>();
    builder.Services.AddSingleton<HealthCheckQueryHandler>();
});

minimalWebApp?.Start();

