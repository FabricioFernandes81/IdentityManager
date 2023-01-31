using Microsoft.AspNetCore.Identity;

namespace IndentityServer.Context
{
    public class AppAplicationRoles : IdentityRole
    {
        public string Description { get; set; }
    }
}
