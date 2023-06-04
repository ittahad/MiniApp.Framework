using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MinimalFramework;
using MinimalWebApi;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TestingWebApi;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Meter meter = new Meter("HatCo.HatStore", "1.0.0");
Counter<int> s_hatsSold = meter.CreateCounter<int>(name: "hats-sold",
                                                            unit: "Hats",
                                                            description: "The number of hats sold in our store");

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

    // JWT confs
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

    builder.Services.AddSingleton<ITokenService, TokenService>();
    builder.Services.AddSingleton<HealthCheckQueryHandler>();

    //Metrics
    builder.Services.AddOpenTelemetryMetrics(options =>
    {
        options
           .AddMeter("HatCo.HatStore");

        options.AddPrometheusExporter(options =>
        {
            options.StartHttpListener = true;
            options.HttpListenerPrefixes = new string[] { $"http://localhost:9090/" };
        });
    });

    Task.Factory.StartNew(() =>
    {

        Console.WriteLine("metrics thread...");
        while (true)
        {
            // Pretend our store has a transaction each second that sells 4 hats
            Thread.Sleep(1000);
            s_hatsSold.Add(4);
        }
    });

});



minimalWebApp?.Start(app =>
{
    app.UseOpenTelemetryPrometheusScrapingEndpoint();
});

