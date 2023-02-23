using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class ClientsSecretsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Type")]
        public string? Type { get; set; }

       
        [Display(Name = "Description")]
        public string? DescriptionSecret { get; set; }

        [Display(Name = "Valor")]
        public string? Valor { get; set; }

        [Display(Name = "Expiration")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Expiration { get; set; }
    }
}
