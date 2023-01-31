using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer.Context;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Data
{
    public class ProfileAppService : IProfileService
    {
        private readonly UserManager<AppAplicationUser> _userManager;
        private readonly RoleManager<AppAplicationRoles> _roleManager;
        private readonly IUserClaimsPrincipalFactory<AppAplicationUser> _userClaimsPrincipalFactory;

        public ProfileAppService(UserManager<AppAplicationUser> userManager, RoleManager<AppAplicationRoles> roleManager, IUserClaimsPrincipalFactory<AppAplicationUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string id = context.Subject.GetSubjectId();



            AppAplicationUser user = await _userManager.FindByIdAsync(id);

            ClaimsPrincipal Userclaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claims = Userclaims.Claims.ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FullName));

            if (_userManager.SupportsUserRole) 
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {

                    claims.Add(new Claim(JwtClaimTypes.Role, role));

                    if (_roleManager.SupportsRoleClaims)
                    {

                        AppAplicationRoles identityRole = await _roleManager.FindByIdAsync(role);

                        if (identityRole != null)
                        {
                            claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                        }

                    }

                }

            }
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string userId = context.Subject.GetSubjectId();
            AppAplicationUser user = await _userManager.FindByIdAsync(userId);

            context.IsActive = user is not null;
        }
    }
}
