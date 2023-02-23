using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class UriClientsViewModel
    {
        
        public Guid Id { get; set; }

        public string? Uri { get; set; }
        public bool CallSing { get; set; }

        public Guid? ClientId { get; set; }
        public virtual Clients? Clients { get; set; }
    }
}
