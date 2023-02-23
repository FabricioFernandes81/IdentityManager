using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class ClientSecrets
    {
        [Key]
        public Guid Id { get; set; }

        public string Type { get; set; }
        public string? Secret { get; set; }

        public string? DescriptionSecret { get; set; }

        public string? Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Expiration { get; set; }

        [ForeignKey(nameof(Clients))]
        public Guid? ClientId { get; set; }
        public virtual Clients? Clients { get; set; }
    }
}
