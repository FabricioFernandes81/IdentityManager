using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class ResourceClients
    {
      //  [Key]
        public string? ClienId { get; set; }

     //   [ForeignKey(nameof(Clients))]
        public Guid? ClientId { get; set; }
        public virtual Clients? Clients { get; set; }
    }
}
