using Microsoft.Extensions.Configuration;
using MiniApp.Core;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace MiniApp.PgSQL
{
    public class PgSqlAppTenantContext : IAppTenantContext<ApplicationTenantPgSql>
    {
        private readonly List<ApplicationTenantPgSql> _tenants;

        public PgSqlAppTenantContext(IConfiguration configuration)
        {
            var connectionString = configuration["DbConnectionString"];

            _tenants = GetAllTenants(connectionString);
        }

        public List<ApplicationTenantPgSql> GetAllTenants(string connectionString)
        {
            connectionString += ";Database=Tenants";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            var dataSource = dataSourceBuilder.Build();

            var conn = dataSource.OpenConnection();

            using (var cmd = new NpgsqlCommand($"SELECT * FROM {typeof(ApplicationTenant).Name}s", conn))

            using (var reader = cmd.ExecuteReader())
            {
                var tenantList = new List<ApplicationTenantPgSql>();
                while (reader.Read())
                {
                    var entityType = typeof(ApplicationTenantPgSql);
                    var appTenantObj = new Dictionary<string, object>();
                    entityType.GetProperties().ToList().ForEach(p =>
                    {
                        var propName = p.Name;
                        var propValue = reader.GetValue(propName);
                        if(!appTenantObj.ContainsKey(propName))
                            appTenantObj.Add(propName, propValue);
                    });

                    var serializedObj = JsonSerializer.Serialize(appTenantObj);
                    var parsedObj = JsonSerializer.Deserialize<ApplicationTenantPgSql>(serializedObj);
                    tenantList.Add(parsedObj!);
                }
                return tenantList;
            }
        }

        public ApplicationTenantPgSql GetApplicationTenant(string origin)
        {
            return _tenants.FirstOrDefault(x => x.Origin == origin)!;
        }

        public ApplicationTenantPgSql GetApplicationTenantById(string tenantId)
        {
            return _tenants.FirstOrDefault(x => x.TenantId == tenantId)!;
        }
    }
}