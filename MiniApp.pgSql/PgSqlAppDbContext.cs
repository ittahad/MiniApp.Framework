using Microsoft.Extensions.Configuration;
using MiniApp.Core;
using Npgsql;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace MiniApp.PgSQL
{
    public class PgSqlAppDbContext : IAppDbContext
    {
        private readonly Dictionary<string, NpgsqlDataSource> _connections 
            = new Dictionary<string, NpgsqlDataSource>();

        private readonly IAppTenantContext<ApplicationTenantPgSql> _tenantContext;

        public PgSqlAppDbContext(
            IConfiguration configuration,
            IAppTenantContext<ApplicationTenantPgSql> tenantContext)
        {
            var connectionString = configuration["DbConnectionString"];

            _tenantContext = tenantContext;

            LoadAllDbContexts(connectionString);
        }

        public void LoadAllDbContexts(string connectionString)
        {
            var tenants = _tenantContext.GetAllTenants(connectionString);

            tenants.ForEach(tenant =>
            {
                connectionString = $"{connectionString};Database=" + tenant.TenantId;

                var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

                var dataSource = dataSourceBuilder.Build();

                _connections.Add(tenant.TenantId!, dataSource);
            });
        }

        public async Task<bool> DeleteItem<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            using (var src = _connections[tenant])
            {
                var connection = await src.OpenConnectionAsync();

                var tableName = typeof(T).Name;
                var whereClause = SqlExpressionHelper.GetWhereClause(expression);

                var sql = $"DELETE FROM {tableName} WHERE {whereClause}";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    var affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        public async Task<T> GetItem<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            return (await GetItems<T>(expression, tenant)).FirstOrDefault()!;
        }

        public async Task<IEnumerable<T>> GetItems<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            using (var src = _connections[tenant])
            {
                var connection = await src.OpenConnectionAsync();

                var tableName = typeof(T).Name;
                var whereClause = SqlExpressionHelper.GetWhereClause(expression);

                var sql = $"SELECT * FROM {tableName} WHERE {whereClause}";
                
                using (var cmd = new NpgsqlCommand(sql, connection))

                using (var reader = cmd.ExecuteReader())
                {
                    var dataList = new List<T>();
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
                        dataList.Add(parsedObj!);
                    }
                    return dataList;
                }
            }
        }

        public async Task<bool> SaveItem<T>(T item, string tenant)
        {
            using (var src = _connections[tenant])
            {
                var connection = await src.OpenConnectionAsync();

                var tableName = typeof(T).Name;
                var columns = string.Join(", ", typeof(T).GetProperties().Select(prop => prop.Name)).ToLower();
                var values = string.Join(", ", typeof(T).GetProperties().Select(prop => ParseValue(item, prop)));
                var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    var affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        private static object? ParseValue<T>(T item, PropertyInfo prop)
        {
            var srcType = prop.GetType();

            if(srcType == typeof(string)) return $"'{prop.GetValue(item)}'";
            else if(srcType == typeof(int)) return (int)prop.GetValue(item);
            else if(srcType == typeof(double)) return (float)prop.GetValue(item);
            return $"'{prop.GetValue(item)}'";
        }

        public async Task<bool> Update<T>(Expression<Func<T, bool>> expression, T updateObj, string tenant)
        {
            using (var src = _connections[tenant])
            {
                var connection = await src.OpenConnectionAsync();

                var tableName = typeof(T).Name;
                var setClause = SqlExpressionHelper.GetUpdateSetClause(updateObj);
                var whereClause = SqlExpressionHelper.GetWhereClause(expression);

                var sql = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    var affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }
    }
}