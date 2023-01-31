using IdentityServer.Models;

namespace IdentityServer.Models
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }

        public List<UserRolesViewModel> Permisoes { get; set; }
       // public IEnumerable<string> Permisoes { get; set; }
      //  public string Permisoes { get; set; }
        public bool Status { get;set; }
    }
}
