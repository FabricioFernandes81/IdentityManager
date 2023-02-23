using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace IdentityServer.Controllers
{
    public class IdentityServerController : Controller
    {
        private readonly IIdentityService _service;
        
        public IdentityServerController(IIdentityService service)
        {
            _service = service;
        

        }

        public async Task<IActionResult> Clients()
        {
            var clients = await _service.GetClientsAsync();
            var ClientsviewModels = new List<ClientsViewModel>();

            foreach (var itens in clients )
            {
                var ViewModelClient = new ClientsViewModel()
                {
                    Id = itens.Id,
                    ClientId = itens?.ClientId,
                    ClientName = itens?.ClientName,
                    Description = itens?.Description,
                    ClientUrl = itens?.ClientUrl,
                    ClientLogo = itens?.ClientLogo,
                };
                ClientsviewModels.Add(ViewModelClient);
                
            }

            return View(ClientsviewModels);
         
        }

        [HttpGet]
        public async Task<ActionResult> AddClients()
        {
            var vm = await BiulderClientsView();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddClients(ClientsViewModel model)
        {
          //  if (ModelState.IsValid) 
          //  {
                await _service.CreateClient(model);
                //redireciona 
                return RedirectToAction("Clients", "IdentityServer");
            //  }
          //  var vm = await BiulderClientsView();
          //  return View(vm);
            //     return View(model);
        }
        [HttpGet]
        public IActionResult EditClients(int id)
        {
            return Json("Editar clientes");
        }
        [HttpPost]
        public IActionResult EditClients(int id, string model)
        {
            return Json("Editar clientes");
        }

        [HttpGet]
        public IActionResult DeleteClients(int id)
        {
            return Json("Deletar clientes");
        }
        [HttpPost, ActionName("DeleteClients")]
        public IActionResult ClientsConfirmed(int id)
        {
            return Json("Deletar clientes");
        }

        private async Task<ClientsViewModel> BiulderClientsView()
        {
            var apisResouce = await _service.GetResourcesAsync(null);
            ClientsViewModel client = new ClientsViewModel();
            
        /*    client.apiResourceViews = new List<ApiResourceViewModel>();
            
            client.apiResourceViews = new List<ApiResourceViewModel>();*/

            foreach (var item in apisResouce)
            {
                var ViewApiResource = new ApiResourceViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Display = item.Display,
                    Descricao = item.Descricao,
                    IsDescoberta = item.IsDescoberta
                   
                };
                client.apiResourceViews.Add(ViewApiResource);
            }
        
            return client;

        }

    }
}
