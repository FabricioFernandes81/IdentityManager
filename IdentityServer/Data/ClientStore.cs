using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Data
{
    public class ClientStore : IClientStore
    {
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            throw new NotImplementedException();
        }
    }
}
