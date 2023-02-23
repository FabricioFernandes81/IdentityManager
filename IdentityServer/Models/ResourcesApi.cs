using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class ResourcesApi
    {
        [Key]
        public Guid? Id { get; set; }

        public string? Name { get; set; }
        public string? Display { get; set; }
        public string? Descricao { get; set; }

        public Guid? ResourceId { get; set; }
      

        public bool? IsDescoberta { get; set; }

       
    }
}
