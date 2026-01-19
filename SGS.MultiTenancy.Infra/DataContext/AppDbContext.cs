using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Entities.Common;

namespace SGS.MultiTenancy.Infra.DataContext
{
    public class AppDbContext : DbContext
    {
        private readonly Guid _tenantId;

        protected AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _tenantId = Guid.Empty;
        }

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider?.TenantId ?? Guid.Empty;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            // ✅ Apply global tenant filter
            modelBuilder.ApplyTenantFilter(_tenantId);
        }
    }
}
