using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Context
{
    public class AppDbContext : IdentityDbContext <AppAplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 

        }
        public DbSet<AppAplicationRoles> AspNetRoles { get; set; }

        public DbSet<NavigationMenu> NavigationMenu { get; set; }
        public DbSet<PermissionMenuRole> PermissionMenuRole { get; set; }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<ClientScopes> ClientScopes { get; set; }
        public DbSet<UriClients> UriClients { get; set; }

        public DbSet<ClientSecrets> ClientSecrets { get; set; }
        
        
        public DbSet<ResourcesApi> ResourcesApis { get; set; }

        public DbSet<ResourceClients> ResourceClients { get; set; }

        public DbSet<ClientGrantTypes> ClientGrantTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PermissionMenuRole>().HasKey(c => new { c.RoleId, c.NavigationMenuId });
            builder.Entity<ResourceClients>().HasKey(c => new { c.ClienId, c.ClientId });

            foreach (var relacions in builder.Model.GetEntityTypes().SelectMany(e=> e.GetForeignKeys()))
                {
                relacions.DeleteBehavior = DeleteBehavior.Restrict;
                }

            base.OnModelCreating(builder);
        }

    }

}
