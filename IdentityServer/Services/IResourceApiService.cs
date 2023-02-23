using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface IResourceApiService
    {
        Task<List<ResourcesApi>> GetApiResourceAllAsync();
    }
}