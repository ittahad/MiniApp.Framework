using Microsoft.Extensions.DependencyInjection;
using MiniApp.Core;

namespace MiniApp.MongoDB
{
    public static class MongoDBExtensions
    {
        public static void AddMongoDB(this IServiceCollection container) 
        {
            container.AddSingleton<IAppDbContext, MongoAppDbContext>();
            
            container.AddSingleton<
                IAppTenantContext<ApplicationTenantMongoDb>, 
                MongoAppTenantContext<ApplicationTenantMongoDb>>();
        }
    }
}
