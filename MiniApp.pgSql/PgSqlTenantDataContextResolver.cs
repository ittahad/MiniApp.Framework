using MiniApp.PgSQL;
using System.Reflection;

namespace MiniApp.Core
{
    public class PgSqlTenantDataContextResolver : ITenantDataContextResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppTenantContextInfo _contextInfo;

        public PgSqlTenantDataContextResolver(
            IServiceProvider serviceProvider,
            AppTenantContextInfo contextInfo)
        {
            _serviceProvider = serviceProvider;
            _contextInfo = contextInfo;
        }

        public List<ApplicationTenant> GetAllTenants(string connectionString)
        {
            var _t = _contextInfo.ServiceType;

            Type serviceType = typeof(IAppTenantContext<>).MakeGenericType(_t!);

            var ins = _serviceProvider.GetService(serviceType);

            var appTenants = (List<ApplicationTenantPgSql>?)ins!.GetType()!.GetMethod(MethodBase.GetCurrentMethod()!.Name)!
                .Invoke(ins, new object[]{ connectionString });
            
            return appTenants!.Cast<ApplicationTenant>().ToList();
        }

        public ApplicationTenant GetApplicationTenant(string origin)
        {
            var _t = _contextInfo.ServiceType;

            Type serviceType = typeof(IAppTenantContext<>).MakeGenericType(_t!);

            var ins = _serviceProvider.GetService(serviceType);

            ApplicationTenant? appTenant = (ApplicationTenantPgSql?)ins!.GetType()!.GetMethod(MethodBase.GetCurrentMethod()!.Name)!
                .Invoke(ins, new object[]{ origin });
            
            return appTenant!;
        }

        public ApplicationTenant GetApplicationTenantById(string tenantId)
        {
            var _t = _contextInfo.ServiceType;

            Type serviceType = typeof(IAppTenantContext<>).MakeGenericType(_t!);

            var ins = _serviceProvider.GetService(serviceType);

            ApplicationTenant? appTenant = (ApplicationTenantPgSql?)ins!.GetType()!.GetMethod(MethodBase.GetCurrentMethod()!.Name)!
                .Invoke(ins, new object[]{ tenantId });
            
            return appTenant!;
        }
    }
}
