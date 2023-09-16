using System.Runtime.InteropServices;

namespace MiniApp.Core
{
    public abstract class ApplicationTenant
    {
        public Guid? ItemId { get; set; }
        public string? TenantId { get; set; }
        public string? PasswordSecret { get; set; }
        public string? Origin { get; set; }
    }
}
