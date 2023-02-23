using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class ApiResourceViewModel
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }
        public string? Display { get; set; }
        public string? Descricao { get; set; }

        public bool? IsDescoberta { get; set; }

        [NotMapped]
        public bool IsPermitted { get; set; }
        public Guid? ClientId { get; set; }
        
    }
}
