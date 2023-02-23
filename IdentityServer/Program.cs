using IdentityServer.Services;
using IdentityServer.Utils;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer.Context;
using IdentityServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*Conexão com Banco de Dados*/
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlServer(connectionString: @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Supermercado;Integrated Security=True"
));

/* */
builder.Services.AddIdentity<AppAplicationUser, AppAplicationRoles>( options=>
{
    options.Lockout.AllowedForNewUsers = true;
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    options.Lockout.MaxFailedAccessAttempts = 3;
})
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddIdentityServer()
      .AddInMemoryIdentityResources(Config.IdentityResources)
      .AddInMemoryApiScopes(Config.ApiScopes)
      .AddInMemoryClients(Config.Clients)
      .AddAspNetIdentity<AppAplicationUser>()
      .AddDeveloperSigningCredential();


    
/* Scheme Autenticação */
builder.Services.AddAuthentication()
    .AddFacebook("Facebook", options =>
    {
        //  var facebookAuth = builder.Configuration.GetSection("Authentication:Facebook");
          options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        
        options.ClientId = "5727659793966412";
        options.ClientSecret = "b685428780dc19aa4177b8ae86e9d808";

    })
    .AddGoogle("Google",options=> 
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = "1031916502668-q06po3gcfcgqk1io7iv6jgcib1leb9ta.apps.googleusercontent.com";
        options.ClientSecret = "b685428780dc19aa4177b8ae86e9d808GOCSPX-7SIGJ7pGjNhpaP9QpeOCHzWLfeVo";

    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<DatabaseIdentityServer>();

builder.Services.AddScoped<IProfileService, ProfileAppService>();

builder.Services.AddScoped<IAccesService, AccessService>();
builder.Services.AddScoped<IResourceApiService, ResourceApiService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();
app.UseAuthentication();


SeedDatabaseIdentityServer(app);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedDatabaseIdentityServer(IApplicationBuilder app) 
{
 using (var serviceScope = app.ApplicationServices.CreateScope()) 
    {
        var initRolesUser = serviceScope.ServiceProvider.GetService<DatabaseIdentityServer>();

        initRolesUser.InicializeRoles();
        initRolesUser.InicializeUserMaster();
    }
}