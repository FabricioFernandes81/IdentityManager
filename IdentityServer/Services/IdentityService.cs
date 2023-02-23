using Elfie.Serialization;
using IdentityServer.Context;
using IdentityServer.Models;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace IdentityServer.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly AppDbContext _dbContext;

        public IdentityService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Clients>> GetClientsAsync()
        {
            var itens = await _dbContext.Clients.Select(p => new Clients()
            {
                Id = p.Id,
                ClientId = p.ClientId,
                ClientName = p.ClientName,
                Description = p.Description,
                ClientUrl = p.ClientUrl,
                ClientLogo = p.ClientLogo
            }).ToListAsync();

            return itens;
        }

        public async Task<List<ApiResourceViewModel>> GetResourcesAsync(string? Id)
        {

            var itens = await (from m in _dbContext.ResourcesApis
                               join rm in _dbContext.ResourceClients
                               on new { X1 = m.Id, X2 = Id } equals new { X1 = rm.ClientId, X2 = rm.ClienId }
                               into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new ApiResourceViewModel()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Display = m.Display,
                                   Descricao = m.Descricao,
                                   IsDescoberta = m.IsDescoberta,
                                   IsPermitted = rm.ClienId == Id

                               }).AsNoTracking().ToListAsync();
            return itens;


        }

        public async Task CreateClient(ClientsViewModel clients)
        {
            Clients clients1 = new Clients()
            {
                Id = Guid.NewGuid(),
                ClientId = clients.ClientId,
                ClientName = clients.ClientName,
                Description = clients.Description,
                ClientUrl = clients.ClientUrl,
                ClientLogo = clients.ClientLogo,
                RequestConsent = clients.RequestConsent
            
            };
             _dbContext.Clients.Add(clients1);
            if (clients.UriClient.Count > 0)
            {
                await CreateUrlClients(clients.UriClient, (Guid)clients1.Id);
            }
           await CreateClientSecret(clients.clientsSecrets,(Guid)clients1.Id);
           await CreateClientResourcesApi(clients.apiResourceViews, clients1.Id.ToString());

           await CreateGrantTypes(clients.Client_AllowedGrantTypes, (Guid)clients1.Id);

           await _dbContext.SaveChangesAsync();

            
        }

        private async Task CreateClientSecret(ClientsSecretsViewModel clientSecretsModel, Guid ClientId)
        {
            ClientSecrets clientSecret = new ClientSecrets()
            {
                Id = Guid.NewGuid(),
                Type = clientSecretsModel.Type,
                DescriptionSecret = clientSecretsModel.DescriptionSecret,
                Secret = clientSecretsModel.Valor,
                Expiration = clientSecretsModel.Expiration,
                ClientId = ClientId
            };
            _dbContext.ClientSecrets.Add(clientSecret);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateGrantTypes(ClientGrantTypesViewModel[]? clientGrantTypes, Guid ClientId)
        {
            int lenght = clientGrantTypes.Count();
            for (int i = 0; i < lenght; i++)
            {
                if (clientGrantTypes[i].GrandType.ToString() != string.Empty)
                {

                    ClientGrantTypes ListclientGrantTypes = new ClientGrantTypes()
                    {

                        GrandType = clientGrantTypes[i].GrandType.ToString(),
                        ClientId = ClientId

                    };
                    _dbContext.ClientGrantTypes.Add(ListclientGrantTypes);

                }
            }
            await _dbContext.SaveChangesAsync();

        }
        private async Task<bool> CreateClientResourcesApi(List<ApiResourceViewModel> viewModelsApiResource, string ClientId) 
        {
            if (string.IsNullOrWhiteSpace(ClientId)) return false;
            var existResourceClient = await _dbContext.ResourceClients.Where(r => r.ClienId == ClientId).ToListAsync();
            _dbContext.RemoveRange(existResourceClient);
            foreach (var item in viewModelsApiResource)
            {
                await _dbContext.ResourceClients.AddAsync(new ResourceClients
                {
                   ClienId = item.Name,
                   ClientId = new Guid(ClientId),
                });

            }
            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }
        private async Task<bool> CreateUrlClients(List<UriClientsViewModel>? modelUrls,Guid ClientId)
        {
            foreach (var item in modelUrls)
            {
                UriClients uriClients = new UriClients()
                {
                    Id = Guid.NewGuid(),
                    ClientId = ClientId,
                    Uri = item.Uri,
                    callSing = item.CallSing,
                    
                    
                };
                _dbContext.UriClients.Add(uriClients);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private Task<Client> GetClientById()
        {
            return null;
        }
    }
}
