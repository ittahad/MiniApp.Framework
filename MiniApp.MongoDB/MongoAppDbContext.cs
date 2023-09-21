using Microsoft.Extensions.Configuration;
using MiniApp.Core;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MiniApp.MongoDB
{
    public class MongoAppDbContext : IAppDbContext
    {
        private readonly Dictionary<string, IMongoDatabase> _dbList;

        public MongoAppDbContext(
            IConfiguration configuration,
            IAppTenantContext<ApplicationTenantMongo> appTenantContext)
        {
            var connectionString = configuration["DbConnectionString"];

            var allTenants = appTenantContext.GetAllTenants(connectionString);

            var client = new MongoClient(connectionString);

            _dbList = new Dictionary<string, IMongoDatabase>();

            allTenants.ForEach(tenant =>
            {
                var db = client.GetDatabase(tenant.TenantId);

                _dbList.Add(tenant.TenantId!, db);
            });
        }

        public async Task<bool> DeleteItem<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            var result = await _dbList[tenant].GetCollection<T>(typeof(T).Name).DeleteManyAsync<T>(expression);

            return result.DeletedCount > 0;
        }

        public async Task<T> GetItem<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            var foundItems = await _dbList[tenant].GetCollection<T>(typeof(T).Name).FindAsync<T>(expression);

            return foundItems.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetItems<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            var foundItems = await _dbList[tenant].GetCollection<T>(typeof(T).Name).FindAsync<T>(expression);

            return foundItems.ToEnumerable();
        }

        public async Task<bool> SaveItem<T>(T item, string tenant)
        {
            await _dbList[tenant].GetCollection<T>(typeof(T).Name).InsertOneAsync(item);

            return true;
        }

        public async Task<bool> Update<T>(Expression<Func<T, bool>> expression, T updateObj, string tenant)
        {
            UpdateDefinition<T>? updateDefBuilder = null;

            updateObj!.GetType().GetProperties().ToList()
                .ForEach(prop =>
                {
                    updateDefBuilder = updateDefBuilder.Set(prop.Name, prop.GetValue(updateObj));
                });

            var result = await _dbList[tenant].GetCollection<T>(typeof(T).Name).UpdateOneAsync(expression, updateDefBuilder, new UpdateOptions() { IsUpsert = true});

            return result.IsAcknowledged;
        }
    }
}