using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Context
{
    public class AppAplicationRoles : IdentityRole
    {
        public string Description { get; set; }
    }
}
