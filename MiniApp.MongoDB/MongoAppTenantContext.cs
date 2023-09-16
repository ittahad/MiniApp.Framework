using Microsoft.Extensions.Configuration;
using MiniApp.Core;
using MongoDB.Driver;

namespace MiniApp.MongoDB
{
    public class MongoAppTenantContext<T> : IAppTenantContext<T> where T : ApplicationTenant
    {
        private readonly List<T> _tenants;

        public MongoAppTenantContext(IConfiguration configuration)
        {
            var connectionString = configuration["DbConnectionString"];

            _tenants = GetAllTenants(connectionString);
        }

        public List<T> GetAllTenants(string connectionString)
        {
            var client = new MongoClient(connectionString);

            var tenantDb = client.GetDatabase("Tenants");

            var filter = Builders<T>.Filter.Empty;

            var foundTenants = tenantDb.GetCollection<T>($"{typeof(ApplicationTenant).Name}s").Find(filter);
            
            return foundTenants.ToList();
        }

        public T GetApplicationTenant(string origin)
        {
            return _tenants.FirstOrDefault(x => x.Origin == origin)!;
        }

        public T GetApplicationTenantById(string tenantId)
        {
            return _tenants.FirstOrDefault(x => x.TenantId == tenantId)!;
        }
    }
}