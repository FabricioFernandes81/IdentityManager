using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class Clients
    {
        [Key]
        public Guid? Id { get; set; }
        public string? ClientId { get; set; }
        
        public string? ClientName { get; set; }

        public string? Description { get; set; }
        
        public string? ClientUrl { get; set; }
        public string? ClientLogo { get; set;}
        public bool RequestConsent { get; set; }

    }
}
