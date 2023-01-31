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
                if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c") });
                }

               /* if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c") });
                }
                if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c") });
                }
                if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
                {
                    _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c") });
                }*/
                /*       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c")))
                       {
                           _dbContext.PermissionMenuRole.Add(new PermissionMenuRole() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c") });
                       }

                       if (!_dbContext.PermissionMenuRole.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
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
                       }
                  //     _dbContext.SaveChanges();*/
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

                },
                new NavigationMenu()
                {
                    Id = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    Name = "Usuarios",
                    ControllerName = "ManageUser",
                    ActionName = "Users",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),

                },
                new NavigationMenu()
                {
                    Id = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734"),
                    Name = "Adicionar Usuários",
                    ControllerName = "ManageUser",
                    ActionName = "CreateUser",
                    ParentMenuId = null //new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),

                },
                 new NavigationMenu()
                {
                    Id = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5"),
                    Name = "Editar Usuário",
                    ControllerName = "ManageUser",
                    ActionName = "EditUser",
                    ParentMenuId = null //new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),

                },
                  new NavigationMenu()
                {
                    Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    Name = "Deletar Usuário",
                    ControllerName = "ManageUser",
                    ActionName = "DeleteUser",
                    ParentMenuId = null //new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),

                },
                 /*new NavigationMenu()
                {
                    Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    Name = "Permisões",
                    ControllerName = "ManageUser",
                    ActionName = "Roles",
                    ParentMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),

                }*/
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
