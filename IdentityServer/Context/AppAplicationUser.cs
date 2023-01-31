using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Context
{
    public class AppAplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
