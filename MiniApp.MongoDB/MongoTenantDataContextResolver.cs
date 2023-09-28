using MiniApp.Core;
using MongoDB.Driver.Core.Configuration;

namespace MiniApp.MongoDB
{
    public class MongoTenantDataContextResolver : ITenantDataContextResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppTenantContextInfo _contextInfo;

        public MongoTenantDataContextResolver(
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

            var appTenants = (List<ApplicationTenantMongo>?)ins!.GetType()!.GetMethod("GetAllTenants")!
                .Invoke(ins, new object[]{ connectionString });

            return appTenants!.Cast<ApplicationTenant>().ToList();
        }

        public ApplicationTenant GetApplicationTenant(string origin)
        {
            var _t = _contextInfo.ServiceType;

            Type serviceType = typeof(IAppTenantContext<>).MakeGenericType(_t!);

            var ins = _serviceProvider.GetService(serviceType);

            var appTenant = (ApplicationTenant?) ins!.GetType()!.GetMethod("GetApplicationTenant")!
                .Invoke(ins, new object[]{ origin });

            return appTenant!;
        }

        public ApplicationTenant GetApplicationTenantById(string tenantId)
        {
            var _t = _contextInfo.ServiceType;

            Type serviceType = typeof(IAppTenantContext<>).MakeGenericType(_t!);

            var ins = _serviceProvider.GetService(serviceType);

            var appTenant = (ApplicationTenant?) ins!.GetType()!.GetMethod("GetApplicationTenantById")!
                .Invoke(ins, new object[]{ tenantId });

            return appTenant!;
        }
    }
}
