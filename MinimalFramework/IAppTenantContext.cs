namespace MiniApp.Core
{
    public interface IAppTenantContext<T>
    {
        public List<T> GetAllTenants(string connectionString);

        public T GetApplicationTenant(string origin); 

        public T GetApplicationTenantById(string tenantId); 
    }
}
