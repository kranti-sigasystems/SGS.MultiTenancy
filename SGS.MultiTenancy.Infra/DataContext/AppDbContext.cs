using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Entities.Common;

namespace SGS.MultiTenancy.Infra.DataContext
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(e => e.Slug).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            // ----- RELATIONSHIP TABLES -----

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleID, rp.PermissionID });

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleID);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionID);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(ur => new { ur.UserID, ur.RoleID });

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserID);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleID);
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(ua => new { ua.UserID, ua.AddressId });

                entity.HasOne(ua => ua.User)
                      .WithMany(u => u.UserAddresses)
                      .HasForeignKey(ua => ua.UserID);

                entity.HasOne(ua => ua.Address)
                      .WithMany(a => a.UserAddresses)
                      .HasForeignKey(ua => ua.AddressId);
            });

            modelBuilder.ApplyTenantFilter(_tenantProvider);
        }

    }
}