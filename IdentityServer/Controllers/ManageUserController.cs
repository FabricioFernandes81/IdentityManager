using IdentityModel;
using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer.Context;
using IdentityServer.Data;
using IdentityServer.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static IdentityServer4.Models.IdentityResources;
using System.Collections.Generic;

namespace IdentityServer.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly UserManager<AppAplicationUser> _userManager;
        private readonly RoleManager<AppAplicationRoles> _roleManager;
        private readonly IAccesService _accesService;
        public ManageUserController(UserManager<AppAplicationUser> userManager, RoleManager<AppAplicationRoles> roleManager, IAccesService accesService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accesService = accesService;
        }
        [HttpGet]
        [Authorize("Users")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModel = new List<UsersViewModel>();

            foreach (var user in users)
            {
                var ViewModelUser = new UsersViewModel();

                ViewModelUser.Id = user.Id;
                ViewModelUser.Nome = user.FullName;
                ViewModelUser.Email = user?.Email;

                //ViewModelUser.Permisoes = await GetUserRoles(user);

                ViewModelUser.Celular = user.PhoneNumber;
                ViewModelUser.Status = Convert.ToBoolean(user.LockoutEnabled);

                userViewModel.Add(ViewModelUser);
            }


            return View(userViewModel);
        }

   
        [HttpGet]
        public async Task<IActionResult> LockUser(string id)
        {
 
            await _userManager.SetLockoutEnabledAsync(await SearchUser(id), true);
            await _userManager.SetLockoutEndDateAsync(await SearchUser(id), DateTime.UtcNow.AddYears(100));
            return RedirectToAction("Users");

        }
        [HttpGet]
        public async Task<IActionResult> UnlockUser(string id)
        {
            await _userManager.SetLockoutEnabledAsync(await SearchUser(id), false);
            await _userManager.SetLockoutEndDateAsync(await SearchUser(id), null);
            return RedirectToAction("Users");
        }

        [HttpGet]
      //  [Authorize("Roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesViewModel = new List<RolesViewModel>();

            foreach (var role in roles)
            {
                var ViewModelUser = new RolesViewModel();

                ViewModelUser.Id = role.Id;
                ViewModelUser.Nome = role.Name;
                ViewModelUser.Descricao = role.Description;

                rolesViewModel.Add(ViewModelUser);
            }


            return View(rolesViewModel);

        }

        public IActionResult CreateRoles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoles(RolesViewModel rolesmodel)
        {

            AppAplicationRoles Createrole = new AppAplicationRoles();
            Createrole.Name = rolesmodel.Nome;
            Createrole.Description = rolesmodel.Descricao;
            Createrole.NormalizedName = rolesmodel.Nome.ToUpper();

            IdentityResult result = await _roleManager.CreateAsync(Createrole);
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            return View(rolesmodel);


        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            var model = new RolesViewModel
            {
                Id = role.Id,
                Nome = role.Name,
                Descricao = role.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(RolesViewModel rolesmodel)
        {
            var role = await _roleManager.FindByIdAsync(rolesmodel.Id);

            role.Name = rolesmodel.Nome;
            role.Description = rolesmodel.Descricao;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)

            {
                return RedirectToAction("Roles");
            }
            return View(rolesmodel);
        }
        [HttpGet]
        public async Task<IActionResult> EditRolePermission(string id)
        {
            TempData["id"] = id;

            List<MenuViewModel> permissao = new List<MenuViewModel>();
            permissao = await _accesService.GetPermissionsByRoleIdAsync(id);
            
            return View(permissao);
        
        }

        
        [HttpPost]
       
        public async Task<IActionResult> EditRolePermission(string id, List<MenuViewModel> model)
        {
            
                    if (ModelState.IsValid) 
            {

                    var permissaoIds = model.Where(x => x.Permitted).Select(x => x.Id);

                     await _accesService.SetPermissionsRoles(id, permissaoIds);

                     return RedirectToAction(nameof(Roles));
            }
            return View(model);
        }
       
        [Authorize("EditUser")]
        public async Task<IActionResult> EditUser(string id)
        {

            var users = await _userManager.FindByIdAsync(id);
            var userViewModel = new UsersViewModel();

            userViewModel.Id = users.Id;
            userViewModel.Nome = users.FullName;
            userViewModel.Email = users.Email;
            userViewModel.Celular = users.PhoneNumber;
            userViewModel.Permisoes = new List<UserRolesViewModel>();


         
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {

         
                var userRoles = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,


                };
                if (await _userManager.IsInRoleAsync(users, role.Name))
                {
                    userRoles.Selected = true;
                }
                else
                {
                    userRoles.Selected = false;
                }

                userViewModel.Permisoes.Add(userRoles);
            }


            return View(userViewModel);
        }

        [HttpPost]
        [Authorize("EditUser")]
        public async Task<IActionResult> EditUser(UsersViewModel userRolesModel)
        {

            var users = await _userManager.FindByIdAsync(userRolesModel.Id);

            users.FullName = userRolesModel.Nome;
            users.Email = userRolesModel.Email;
            users.NormalizedEmail = userRolesModel.Email.ToUpper();
            users.UserName = userRolesModel.Email;
            users.NormalizedUserName = userRolesModel.Email.ToUpper();
            users.PhoneNumber = userRolesModel.Celular;

            await _userManager.UpdateAsync(users);

            var roles = await _userManager.GetRolesAsync(users);
            var result = await _userManager.RemoveFromRolesAsync(users, roles);

            if (!result.Succeeded)
            {
                //Error 
                return View(userRolesModel);
            }

            result = await _userManager.AddToRolesAsync(users, userRolesModel.Permisoes.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                //Error 
                return View(userRolesModel);
            }

            return RedirectToAction("Users");

        }

        [HttpGet]
        [Authorize("CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
                 var userViewModel = new UsersViewModel();
                 userViewModel.Permisoes = new List<UserRolesViewModel>();


                 foreach (var role in await _roleManager.Roles.ToListAsync())
                 {


                     var userRoles = new UserRolesViewModel
                     {
                         RoleId = role.Id,
                         RoleName = role.Name,


                     };

                     userRoles.Selected = false;


                     userViewModel.Permisoes.Add(userRoles);
                 }

                 return View(userViewModel);
            
        }
        [HttpPost]
        [Authorize("CreateUser")]
        public async Task<IActionResult> CreateUser(UsersViewModel userModel,string senha)
        {

            if (_userManager.FindByEmailAsync(userModel.Email).Result == null)
            {
                AppAplicationUser adduser = new AppAplicationUser()
                {
                    FullName = userModel.Nome,
                    UserName = userModel.Email,
                    NormalizedUserName = userModel.Email.ToUpper(),
                    Email = userModel.Email,
                    NormalizedEmail = userModel.Email.ToUpper(),
                    EmailConfirmed = false,
                    LockoutEnabled = false,
                    PhoneNumber = userModel.Celular,
                    SecurityStamp = Guid.NewGuid().ToString()

                };
            
            IdentityResult resultUser = _userManager.CreateAsync(adduser, senha).Result;

            
                 if (resultUser.Succeeded)
                {
                
                var roles = await _userManager.GetRolesAsync(adduser);
                await _userManager.RemoveFromRolesAsync(adduser, roles);
                await _userManager.AddToRolesAsync(adduser, userModel.Permisoes.Where(x => x.Selected).Select(y => y.RoleName));
                return RedirectToAction("Users");
                }
            }
            return View(userModel);
            
            
        }
        [Authorize("DeleteUser")]
        public async Task<IActionResult>DeleteUser(string? id)
        {

        if(id == null) 
            {
                return NotFound();
            }
            var users = await SearchUser(id);

            var userViewModel = new UsersViewModel();

            userViewModel.Id = users.Id;
            userViewModel.Nome = users.FullName;
            userViewModel.Email = users.Email;
            userViewModel.Celular = users.PhoneNumber;
            userViewModel.Permisoes = new List<UserRolesViewModel>();
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var userRoles = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if (await _userManager.IsInRoleAsync(users, role.Name))
                {
                    userRoles.Selected = true;
                }
                else
                {
                    userRoles.Selected = false;
                }

                userViewModel.Permisoes.Add(userRoles);
            }
            return View(userViewModel);
        }

        [HttpPost,ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id) 
        {
            var user = await SearchUser(id);
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Users");
        }

        private async Task<AppAplicationUser> SearchUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        private async Task<List<string>> GetUserRoles(AppAplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

    }
}
