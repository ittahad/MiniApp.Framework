using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalFramework;
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

            if (_options.UseSwagger.HasValue && _options.UseSwagger.Value)
            {
                builder.Services.AddEndpointsApiExplorer();
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
            }

            builder.Services.AddAuthorization();
            builder.Services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((r, c) =>
                {
                    c.Host("amqps://jvxyncsn:4oLJUAtdt8McmhhdPsjW4AnpqjexG5sQ@sparrow.rmq.cloudamqp.com/jvxyncsn");
                    c.ConfigureEndpoints(r);
                });
            });

            var key = "testKeysd fsdf sdfsdfsdf sdfsdfsdf sdf";
            var issuer = "akash";
            var audience = "*";

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
            builder.Services.AddSingleton<IMinimalMediator, MinimalMediator.MinimalMediator>();
            builder.Services.AddSingleton<IBus>(p => p.GetRequiredService<IBusControl>());

            if (Configure != null)
                Configure(builder);

            var minimalWebApp = builder.Build();

            return new MinimalWebApp(_options) { 
                Application = minimalWebApp
            };
        }
    }
}
