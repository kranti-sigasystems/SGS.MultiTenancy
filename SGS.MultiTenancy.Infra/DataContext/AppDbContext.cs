using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Infra.DataContext
{
    public class AppDbContext : DbContext
    {
        private readonly Guid _tenantId;

        // 👇 Design-time constructor (for migrations)
        protected AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _tenantId = Guid.Empty;
        }

        // 👇 Runtime constructor (for requests)
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
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<MultiTenantAppStatusLog> MultiTenantAppStatusLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            // ✅ Configure MultiTenantAppStatusLog (UPDATED)
            modelBuilder.Entity<MultiTenantAppStatusLog>(b =>
            {
                b.HasKey(x => x.LogId);

                // 🔥 ENUM stored as STRING
                b.Property(x => x.LogLevel)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                b.Property(x => x.Message)
                    .IsRequired();

                // Helpful indexes for log queries
                b.HasIndex(x => x.TenantId);
                b.HasIndex(x => x.TimeStamp);
            });

            // ✅ Apply global tenant filter (UNCHANGED)
            modelBuilder.ApplyTenantFilter(_tenantId);
        }
    }
}
