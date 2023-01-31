using IdentityModel;
using IdentityServer.Models;
using IdentityServer.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Web.Razor.Generator;

namespace IdentityServer.Services
{
    public class AccessService : IAccesService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppAplicationUser> _userManager;
        private readonly RoleManager<AppAplicationRoles> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccessService(AppDbContext dbContext,UserManager<AppAplicationUser> userManager,RoleManager<AppAplicationRoles> roleManager,IHttpContextAccessor httpContextAccessor) 
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> GetMenuItems(ClaimsPrincipal claimsPrincipal, string? controller, string? action)
        {
            var result = false;

            var roleIds = await GetUserRoleIds(claimsPrincipal);

            var data = await (from menu in _dbContext.PermissionMenuRole
                              where roleIds.Contains(menu.RoleId)
                              select menu)
                               .Select(m => m.NavigationMenu)
                               .Distinct()
                               .ToListAsync();
            foreach (var item in data)
            {
                result = (item.ControllerName == controller && item.ActionName == action);
                if (result) 
                {
                    break;
                }
            }


            return result;
               
        }

        public async Task<List<MenuViewModel>> GetPermissionsByRoleIdAsync(string? id)
        {
            var itens = await (from m in _dbContext.NavigationMenu
                               join rm in _dbContext.PermissionMenuRole
                              on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                              into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new MenuViewModel()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   Permitted = rm.RoleId == id
                               }).AsNoTracking().ToListAsync();
            return itens;
        }

        private async Task<List<string>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            //  var externalClaims = ctx.Identity.Claims.Select(c => $"{c.Type}: {c.Value}");
            var clains = ctx.Claims.ToList();
            var userId = GetUserId(ctx);
            var data = await _dbContext.UserRoles.Where(r => r.UserId == userId)
                .Select (r=>r.RoleId)
                .ToListAsync();
            

            return data;
        
        }

        private static string? GetUserId(ClaimsPrincipal user) =>
        (user.Identity) == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(JwtClaimTypes.Subject)?.Value;

        public async Task<bool> SetPermissionsRoles(string? id, IEnumerable<Guid> PermissionIds)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var existRole = await _dbContext.PermissionMenuRole.Where(r => r.RoleId == id).ToListAsync();
            _dbContext.RemoveRange(existRole);
            foreach (var item in PermissionIds)
            {
                await _dbContext.PermissionMenuRole.AddAsync(new PermissionMenuRole()
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });

            }
            var result = await _dbContext.SaveChangesAsync();

            return result > 0;

        }
    }
}

