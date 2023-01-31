using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IAccesService
    {
        Task<List<MenuViewModel>> GetPermissionsByRoleIdAsync(string? id);
        Task<bool> GetMenuItems(ClaimsPrincipal claimsPrincipal, string? controller, string? action);

        Task<bool> SetPermissionsRoles(string? id, IEnumerable<Guid> PermissionIds);
    
    }
}