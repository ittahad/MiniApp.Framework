using Microsoft.Extensions.DependencyInjection;
using MiniApp.Core;

namespace MiniApp.PgSQL
{
    public static class PgSqlExtensions
    {
        public static void AddPgSql(this IServiceCollection container)
        {
            container.AddSingleton<IAppDbContext, PgSqlAppDbContext>();

            container.AddSingleton<
                    IAppTenantContext<ApplicationTenantPgSql>,
                    PgSqlAppTenantContext<ApplicationTenantPgSql>>();
        }
    }
}
