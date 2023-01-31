using Microsoft.AspNetCore.Identity;

namespace IndentityServer.Context
{
    public class AppAplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
