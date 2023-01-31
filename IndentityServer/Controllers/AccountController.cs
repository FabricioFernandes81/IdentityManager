using IdentityModel;
using IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IndentityServer.Context;
using IndentityServer.Data;
using IndentityServer.Models;
using IndentityServer.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

namespace IndentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppAplicationUser> _userManager;
        private readonly SignInManager<AppAplicationUser> _signInManager;
        private readonly RoleManager<AppAplicationRoles> _roleManager;

        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;

        public AccountController(UserManager<AppAplicationUser> userManager, SignInManager<AppAplicationUser> signInManager, RoleManager<AppAplicationRoles> roleManager, IAuthenticationSchemeProvider schemeProvider, IIdentityServerInteractionService interaction, IClientStore clientStore, IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _schemeProvider = schemeProvider;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
        }

        [HttpGet]
        public async Task<IActionResult> login(string returnUrl)
        {
            var model = await BuildLoginViewModelAsync(returnUrl);
            if (model.IsExternalLoginOnly)
            {
                RedirectToAction("Challenge", "External", new { scheme = model.ExternalLoginScheme, returnUrl });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            string button = "login";
            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync
                     (
                     model.Username,
                     model.Password,
                     model.RememberLogin,
                     lockoutOnFailure: true
                     );

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };
                    var isuser = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.UserName
                    };

                    await HttpContext.SignInAsync(isuser, props);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }
                if (result.IsLockedOut)
                {

                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Usuário Bloqueado", clientId: context?.Client.ClientId));
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }



        public IActionResult logoff()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);
            if (vm.ShowLogoutPrompt == false)
            {
                return await Logout(vm);
            }

            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);
            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);


                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return RedirectToAction("index", "home");

        }

        public async Task<IActionResult> Register(string returnUrl)
        {
            var model = await BuildRegisterViewModelAsync(returnUrl);
            if (model.IsExternalLoginOnly)
            {
                RedirectToAction("Challenge", "External", new { scheme = model.ExternalLoginScheme, returnUrl });
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel registerViewModel, string button)
        {
            var context = await _interaction.GetAuthorizationContextAsync(registerViewModel.ReturnUrl);
            if (button != "Register")
            {
                if (context != null)
                {

                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                    if (context.IsNativeClient())
                    {
                        return this.LoadingPage("Redirect", registerViewModel.ReturnUrl);
                    }
                    return Redirect(registerViewModel.ReturnUrl);
                }else{
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                //Função de Registro do Usuário
                if (_userManager.FindByEmailAsync(registerViewModel.Email).Result == null)
                {
                    AppAplicationUser registerUser = new AppAplicationUser()
                    {
                        FullName = registerViewModel.FullNome,
                        UserName = registerViewModel.Email,
                        NormalizedUserName = registerViewModel.Email.ToUpper(),
                        Email = registerViewModel.Email,
                        NormalizedEmail = registerViewModel.Email.ToUpper(),
                        EmailConfirmed = false,
                        LockoutEnabled = false,
                        PhoneNumber = null,
                        SecurityStamp = Guid.NewGuid().ToString()

                    };
                    IdentityResult resultUser = _userManager.CreateAsync(registerUser, registerViewModel.Password).Result;

                    if (resultUser.Succeeded)
                    {
                        _userManager.AddToRoleAsync(registerUser, IdentityConfiguration.Client).Wait();

                        var adminClaims = _userManager.AddClaimsAsync(registerUser, new Claim[]
                           {
                        new Claim(JwtClaimTypes.Name, $"{registerUser.FullName}"),
                        new Claim(JwtClaimTypes.Email, registerUser.Email),
                        new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
                        }).Result;
                    }                   
                }
                
            }
            var register = await BuildRegisterViewModelAsync(registerViewModel.ReturnUrl);
            return View(register);

        }



        /*****************************************/
        /* helper APIs for the Account */
        /*****************************************/
        private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                var vm = new RegisterViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                };
                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }
                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var provider = schemes.Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();
            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        provider = provider.Where(p => client.IdentityProviderRestrictions.Contains(p.AuthenticationScheme)).ToList();

                    }
                }
            }
            return new RegisterViewModel
            {
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ExternalProviders = provider.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };
                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }
                return vm;
            }

            /*Busca a Lista de Provedores Externos*/
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var provider = schemes.Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        provider = provider.Where(p => client.IdentityProviderRestrictions.Contains(p.AuthenticationScheme)).ToList();

                    }
                }
            }


            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = provider.ToArray()
            };
        }
        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };
            if (User?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }
                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }
            return vm;
        }
    }
}
