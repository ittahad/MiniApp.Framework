using Microsoft.Extensions.DependencyInjection;
using MiniApp.Core;

namespace MiniApp.PgSQL
{
    public static class PgSqlExtensions
    {
        public static void AddPgSql(this IServiceCollection container)
        {
            var appTenantType = new AppTenantContextInfo()
            {
                ServiceType = typeof(ApplicationTenantPgSql)
            };

            container.AddSingleton(appTenantType);
            container.AddSingleton<IAppDbContext, PgSqlAppDbContext>();
            container.AddSingleton<ITenantDataContextResolver, PgSqlTenantDataContextResolver>();
            container.AddSingleton(typeof(IAppTenantContext<ApplicationTenantPgSql>), typeof(PgSqlAppTenantContext));
        }
    }
}
