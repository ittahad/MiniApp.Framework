using Microsoft.Extensions.DependencyInjection;
using MiniApp.Core;

namespace MiniApp.MongoDB
{
    public static class MongoDBExtensions
    {
        public static void AddMongoDB(this IServiceCollection container)
        {
            var appTenantType = new AppTenantContextInfo()
            {
                ServiceType = typeof(ApplicationTenantMongo)
            };

            container.AddSingleton(appTenantType);
            container.AddSingleton<IAppDbContext, MongoAppDbContext>();
            container.AddSingleton(typeof(IAppTenantContext<>), typeof(MongoAppTenantContext<>));
        }
    }
}
