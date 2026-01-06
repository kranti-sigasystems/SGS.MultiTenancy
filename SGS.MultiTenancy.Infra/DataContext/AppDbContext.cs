using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using System.Reflection;

namespace SGS.MultiTenancy.Infra.DataContext
{
    public class AppDbContext : DbContext
    {
        private readonly Guid _tenantId;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider?.TenantId ?? Guid.Empty;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip shadow / non-CLR entities
                if (entityType.ClrType == null)
                    continue;

                // Apply only to entities having TenantID : Guid
                var tenantProperty = entityType.FindProperty("TenantID");

                if (tenantProperty != null && tenantProperty.ClrType == typeof(Guid))
                {
                    ApplyTenantFilter(modelBuilder, entityType.ClrType);
                }
            }
        }

        private void ApplyTenantFilter(ModelBuilder modelBuilder, Type entityType)
        {
            var method = typeof(AppDbContext)
                .GetMethod(nameof(SetTenantFilter),
                    BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(entityType);

            method?.Invoke(this, new object[] { modelBuilder });
        }

        private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e =>
                    _tenantId == Guid.Empty ||
                    EF.Property<Guid>(e, "TenantID") == _tenantId);
        }
    }
}
