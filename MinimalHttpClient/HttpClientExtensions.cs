using MiniApp.Core;
using MiniApp.Http;
using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientExtensions
    {
        public static void AddHttpPolicy(this IServiceCollection serviceCollection)
        {
            // Retry policy 
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout, // 504
            };

            var policy1 = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(10));

            serviceCollection.AddHttpClient<IMinimalHttpClient, MinimalHttpClient>()
                .AddPolicyHandler(policy1);
        }
    }
}