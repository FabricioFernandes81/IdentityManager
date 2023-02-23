using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class ClientsViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "ID do Cliente")]
        public string? ClientId { get; set; }
        [Display(Name = "Nome do cliente")]
        public string? ClientName { get; set; }
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
        [Display(Name = "Cliente URI")]
        public string? ClientUrl { get; set; }
        [Display(Name = "Logo URI")]
        public string? ClientLogo { get; set; }
        [Display(Name = "Requer consentimento")]
        public bool RequestConsent { get; set; }

        public List<UriClientsViewModel>? UriClient { get; set; } = new List<UriClientsViewModel>();

        public ClientsSecretsViewModel? clientsSecrets { get; set; }

        public List<ApiResourceViewModel>? apiResourceViews { get; set; } = new List<ApiResourceViewModel>();

        public ClientGrantTypesViewModel[]? Client_AllowedGrantTypes { get; set; }
      
        //  public ClientApiResourceScopesDTO[]? Client_ApiResourceScopes { get; set; }

      

    }
}
