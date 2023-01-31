namespace IdentityServer.Models
{
    public class PermissionMenuRole
    {
        public string? RoleId { get; set; }
        public Guid NavigationMenuId { get; set; }
        public NavigationMenu? NavigationMenu { get; set; }
    }
}
