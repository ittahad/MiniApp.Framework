using Microsoft.Extensions.Configuration;
using MiniApp.Core;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace MiniApp.PgSQL
{
    public class PgSqlAppTenantContext<T> : IAppTenantContext<T> where T : ApplicationTenant
    {
        private readonly List<T> _tenants;

        public PgSqlAppTenantContext(IConfiguration configuration)
        {
            var connectionString = configuration["DbConnectionString"];

            _tenants = GetAllTenants(connectionString);
        }

        public List<T> GetAllTenants(string connectionString)
        {
            connectionString += ";Database=Tenants";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            var dataSource = dataSourceBuilder.Build();

            var conn = dataSource.OpenConnection();

            using (var cmd = new NpgsqlCommand($"SELECT * FROM {typeof(ApplicationTenant).Name}s", conn))

            using (var reader = cmd.ExecuteReader())
            {
                var tenantList = new List<T>();
                while (reader.Read())
                {
                    var entityType = typeof(T);
                    var appTenantObj = new Dictionary<string, object>();
                    entityType.GetProperties().ToList().ForEach(p =>
                    {
                        var propName = p.Name;
                        var propValue = reader.GetValue(propName);
                        if(!appTenantObj.ContainsKey(propName))
                            appTenantObj.Add(propName, propValue);
                    });

                    var serializedObj = JsonSerializer.Serialize(appTenantObj);
                    var parsedObj = JsonSerializer.Deserialize<T>(serializedObj);
                    tenantList.Add(parsedObj!);
                }
                return tenantList;
            }
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