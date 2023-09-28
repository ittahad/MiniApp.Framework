using Microsoft.Extensions.Configuration;
using MiniApp.Core;
using MongoDB.Driver;

namespace MiniApp.MongoDB
{
    public class MongoAppTenantContext : IAppTenantContext<ApplicationTenantMongo>
    {
        private readonly List<ApplicationTenantMongo> _tenants;

        public MongoAppTenantContext(IConfiguration configuration)
        {
            var connectionString = configuration["DbConnectionString"];

            _tenants = GetAllTenants(connectionString);
        }

        public List<ApplicationTenantMongo> GetAllTenants(string connectionString)
        {
            var client = new MongoClient(connectionString);

            var tenantDb = client.GetDatabase("Tenants");

            var filter = Builders<ApplicationTenantMongo>.Filter.Empty;

            var foundTenants = tenantDb.GetCollection<ApplicationTenantMongo>($"{typeof(ApplicationTenant).Name}s").Find(filter);
            
            return foundTenants.ToList();
        }

        public ApplicationTenantMongo GetApplicationTenant(string origin)
        {
            return _tenants.FirstOrDefault(x => x.Origin == origin)!;
        }

        public ApplicationTenantMongo GetApplicationTenantById(string tenantId)
        {
            return _tenants.FirstOrDefault(x => x.TenantId == tenantId)!;
        }
    }
}