using MiniApp.Core;

namespace MiniApp.PgSQL
{
    public class ApplicationTenantPgSql : ApplicationTenant
    {
        public new string? ItemId { get; set; }
        public new string? FirstName { get; set; }
        public new string? LastName { get; set; }
    }
}