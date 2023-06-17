﻿using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalFramework;
using MinimalHttpClient;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace MinimalWebApi
{
    public class MinimalWebAppBuilder
    {
        private MinimalWebAppOptions _options;
        
        public MinimalWebAppBuilder(MinimalWebAppOptions options = null)
        {
            _options = options == null ? new MinimalWebAppOptions() { 
                UseSwagger = false,
                CommandLineArgs = new string[] { },
            } : options;
        }

        public MinimalWebApp Build(
            Action<WebApplicationBuilder> Configure = null)
        {
            var builder = WebApplication.CreateBuilder(_options.CommandLineArgs);

            var serviceName = builder.Configuration.GetSection("ServiceName").Value;

            if (_options.UseSwagger.HasValue && _options.UseSwagger.Value)
            {
                builder.Services.AddControllers();
                builder.Services.AddSwaggerGen(c =>
                {
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "JWT Authentication",
                        Description = "Enter JWT Bearer token ** _only_ **",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { securityScheme, new string[] { } }
                    });
                });
                // this will handle all conventional based routes
                builder.Services.AddSwaggerGenWithConventionalRoutes(options =>
                {
                    options.SkipDefaults = true;
                });
            }

            builder.Host.AddLogger(_options);

            builder.Services.AddAuthorization();

            builder.Services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((r, c) =>
                {
                    c.Host(builder.Configuration["RabbitMqServer"]);
                    c.ConfigureEndpoints(r);
                });

            });
            
            builder.Services.AddHttpPolicy();

            builder.Services.AddSingleton<IMinimalMediator, MinimalMediator.MinimalMediator>();
            builder.Services.AddSingleton<IBus>(p => p.GetRequiredService<IBusControl>());

            // Tracing
            if (_options.OpenTelemetryOptions?.EnableTracing ?? false) 
            {
                builder.Services.AddMinimalOpenTelemetryTracing(_options, serviceName); 
                var MyActivitySource = new ActivitySource(serviceName);
                builder.Services.AddSingleton(MyActivitySource);
            }

            if (Configure != null)
                Configure.Invoke(builder);

            var minimalWebApp = builder.Build();

            return new MinimalWebApp(_options) { 
                Application = minimalWebApp
            };
        }
    }
}
