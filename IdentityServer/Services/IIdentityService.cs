using IdentityServer.Models;
using IdentityServer4.Models;

namespace IdentityServer.Services
{
    public interface IIdentityService
    {
        Task<List<Clients>> GetClientsAsync();

        Task <List<ApiResourceViewModel>> GetResourcesAsync(string? Id);


        Task CreateClient(ClientsViewModel clients);

    }
}
