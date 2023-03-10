using IdentityModel;
using IdentityServer.Models;
using IdentityServer.Utils;
using IdentityServer.Context;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static IdentityServer4.Models.IdentityResources;

namespace IdentityServer.Data
{
    public class DatabaseIdentityServer
    {
        private readonly UserManager<AppAplicationUser> _userManager;
        private readonly RoleManager<AppAplicationRoles> _roleManager;
        private readonly AppDbContext _dbContext;
        public DatabaseIdentityServer(UserManager<AppAplicationUser> userManager, RoleManager<AppAplicationRoles> roleManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public void InicializeRoles() 
        {
         if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
           {
                AppAplicationRoles roleAdmin = new AppAplicationRoles();
                roleAdmin.Name = IdentityConfiguration.Admin;
                roleAdmin.Description = "Full Acess";
                roleAdmin.NormalizedName = IdentityConfiguration.Admin.ToUpper();
                _roleManager.CreateAsync(roleAdmin).Wait();
           }

            if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Client).Result)
            {
                AppAplicationRoles roleClient = new AppAplicationRoles();
                roleClient.Name = IdentityConfiguration.Client;
                roleClient.Description = "Partial access";
                roleClient.NormalizedName = IdentityConfiguration.Client.ToUpper();
                _roleManager.CreateAsync(roleClient).Wait();
            }
            _dbContext.Database.EnsureCreated();

            var permissions = GetPermissions();
            foreach (var item in permissions)
            {
                if (_dbContext?.NavigationMenu?.Any(n => n.Name == item.Name) == false)
                {
                    _dbContext.NavigationMenu.Add(item);
                }
            }

            
           // _dbContext.SaveChanges();
            var _adminRole = _roleManager.Roles.Where(x => x.Name == IdentityConfiguration.Admin).FirstOrDefault();

            if (_adminRole != null)
            {
             if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7CD0D373-C57D-4C70-AA8C-22791983FE1C")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7CD0D373-C57D-4C70-AA8C-22791983FE1C") });
                }

               if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734") });
                }
                if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("283264D6-0E5E-48FE-9D6E-B1599AA0892C")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("283264D6-0E5E-48FE-9D6E-B1599AA0892C") });
                }
                if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5") });
                }
                if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe2C")))
                  {
                   _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe2C") });
                  }

                /*       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
                       {
                           _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c") });
                       }

                       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB")))
                       {
                           _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB") });
                       }

                       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5")))
                       {
                           _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5") });
                       }

                       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734")))
                       {
                           _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734") });
                       }

                       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6")))
                       {
                           _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6") });
                       }*/
                      // _dbContext.SaveChanges();
            }
            var Clientes = CreateClients();
            foreach (var item in Clientes)
            {
                if (_dbContext?.Clients?.Any(n => n.ClientId == item.ClientId) == false)
                {
                    _dbContext.Clients.Add(item);
                }
            }
            
            if (!_dbContext.ClientScopes.Any(p=>p.Id == new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"))) 
            {
                _dbContext.ClientScopes.Add(new ClientScopes() { Id = Guid.NewGuid(),ClientId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),Scope="scope1" });
                
            }
            if (!_dbContext.ClientScopes.Any(p => p.Id == new Guid("13e2f21B-4283-4ff8-bb7a-096e7b89e0f0")))
            {
                _dbContext.ClientScopes.Add(new ClientScopes() { Id = Guid.NewGuid(), ClientId = new Guid("13e2f21B-4283-4ff8-bb7a-096e7b89e0f0"), Scope = "scope1" });
            }

            var ResouceApi = CreateResourceApi();
            foreach (var item in ResouceApi)
            {
                if (_dbContext?.ResourcesApis?.Any(n => n.Name == item.Name) == false)
                {
                    _dbContext.ResourcesApis.Add(item);
                }
            }
            _dbContext.SaveChanges();
        }
        public void InicializeUserMaster()
        { 
                if(_userManager.FindByEmailAsync("admin@com.br").Result == null) 
                {
                AppAplicationUser admin = new AppAplicationUser()
                {
                    FullName = "Foguinho Fernandes",
                    UserName = "admin@com.br",
                    NormalizedUserName = "ADMIN@COM.BR",
                    Email = "admin@com.br",
                    NormalizedEmail = "ADMIN@COM.BR",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "+55 (11) 12345-6789",
                    SecurityStamp = Guid.NewGuid().ToString()

                };
                IdentityResult resultAdmin = _userManager.CreateAsync(admin, "Nunsey#2022").Result;

                if (resultAdmin.Succeeded)
                {
                    _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();

                    var adminClaims = _userManager.AddClaimsAsync(admin,new Claim[]
                       {
                    
                    new Claim(JwtClaimTypes.Name, $"{admin.FullName}"),
                    new Claim(JwtClaimTypes.Email, admin.Email),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                    }).Result;

                }


                }
        
        }
        public static List<Clients> CreateClients()
        {
            return new List<Clients>()
        {
          new Clients()
          {
              Id = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
              ClientId = "teste1",
              ClientName = "Nome Client",
              ClientLogo = "http://localhost:122/logo",
              ClientUrl = "http://localhost:122",
              Description = "Descricção de Api..."

          },
          new Clients()
          {
              Id = new Guid("13e2f21B-4283-4ff8-bb7a-096e7b89e0f0"),
              ClientId = "teste2",
              ClientName = "nome",
              ClientLogo = "http://localhost:134",
              ClientUrl = "http://localhost:127",
              Description = "Decrição Cliente"
          }
          };
        }

        public static List<ResourcesApi> CreateResourceApi()
        {
            return new List<ResourcesApi>()
        {
          new ResourcesApi()
          {
              Id= Guid.NewGuid(),
              Name = "IdentitySerser",
              Display = "API Teste",
              Descricao = "Teste de API",
              IsDescoberta = true,
          },
          new ResourcesApi()
          {
              Id= Guid.NewGuid(),
              Name = "SerserMobile",
              Display = "Client Mobile",
              Descricao = "Mobile API",
              IsDescoberta = true,
          }
         };

        }
        public static List<NavigationMenu> GetPermissions()
        {
            return new List<NavigationMenu>()
            {
                new NavigationMenu()
                {
                    Id = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    Name = "Gerenciamento de Identidade",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    GroupMenu = null,
                    isParent = true,

                },

                 new NavigationMenu()
                {
                    Id = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB"),
                    Name = "Servidor de Identidade",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    GroupMenu = null,
                    isParent = false,

                },
               /*      new NavigationMenu()
                {
                    Id = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657BC"),
                    Name = "Principal",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    GroupMenu = null,
                    isParent = false,

                },*/
                new NavigationMenu()
                {
                    Id = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    Name = "Usuarios",
                    ControllerName = "ManageUser",
                    ActionName = "Users",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    GroupMenu = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    isParent = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734"),
                    Name = "Adicionar Usuários",
                    ControllerName = "ManageUser",
                    ActionName = "CreateUser",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    GroupMenu = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    isParent = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5"),
                    Name = "Editar Usuário",
                    ControllerName = "ManageUser",
                    ActionName = "EditUser",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    GroupMenu = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    isParent = false,
                },
                  new NavigationMenu()
                {
                    Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    Name = "Deletar Usuário",
                    ControllerName = "ManageUser",
                    ActionName = "DeleteUser",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    GroupMenu = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    isParent = false,
                },
                    new NavigationMenu()
                {
                    Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe2C"),
                    Name = "Bloquear / Desbloquear Usuário",
                    ControllerName = "ManageUser",
                    ActionName = "DeleteUser",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    GroupMenu = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    isParent = false,
                },

                  new NavigationMenu()
                {
                    Id = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6"),
                    Name = "Roles",
                    ControllerName = "ManageUser",
                    ActionName = "Roles",
                    ParentMenuId = null,
                    GroupMenu = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    isParent = false,
                },
                
              /*  new NavigationMenu()
                {
                    Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    Name = "Users",
                    ControllerName = "Admin",
                    ActionName = "Users",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),

                },
                
                new NavigationMenu()
                {
                    Id = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5"),
                    Name = "Create Role",
                    ControllerName = "Admin",
                    ActionName = "CreateRole",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),

                },
                new NavigationMenu()
                {
                    Id = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734"),
                    Name = "Edit User",
                    ControllerName = "Admin",
                    ActionName = "EditUser",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),

                },
                new NavigationMenu()
                {
                    Id = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6"),
                    Name = "Edit Role Permission",
                    ControllerName = "Admin",
                    ActionName = "EditRolePermission",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),

                },*/
            };
        }
    }
}
