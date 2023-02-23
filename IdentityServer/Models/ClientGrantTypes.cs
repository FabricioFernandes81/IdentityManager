using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class ClientGrantTypes
    {
        [Key]
        public Guid Id { get; set; }
        public string GrandType { get; set; }
        
        [ForeignKey(nameof(Clients))]
        public Guid? ClientId { get; set; }
        public virtual Clients? Clients { get; set; }
    }
}
