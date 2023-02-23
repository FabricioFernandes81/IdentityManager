using IdentityServer.Context;
using IdentityServer.Models;

namespace IdentityServer.Services
{
    public class ResourceApiService : IResourceApiService
    {
        private readonly AppDbContext _dbContext;

        public ResourceApiService(AppDbContext dbContext) 
        {
         _dbContext = dbContext;
        }

        public Task<List<ResourcesApi>> GetApiResourceAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
