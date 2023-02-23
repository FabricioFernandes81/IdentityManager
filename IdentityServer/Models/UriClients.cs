using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class UriClients
    {
        [Key]
        public Guid Id { get; set; }

        public string? Uri { get; set; }
        public bool callSing { get; set; }

        [ForeignKey(nameof(Clients))]
        public Guid? ClientId { get; set; }
        public virtual Clients? Clients { get; set; }
    }
}
