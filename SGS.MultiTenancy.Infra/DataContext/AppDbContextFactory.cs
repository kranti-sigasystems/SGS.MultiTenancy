using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SGS.MultiTenancy.Core.Application.Interfaces;

namespace SGS.MultiTenancy.Infra.DataContext
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Creates a configured <see cref="AppDbContext"/> instance for design-time operations.
        /// </summary>
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string? connectionString =
                configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<AppDbContext> optionsBuilder =
                new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 44))
            );

            return new AppDbContext(
                optionsBuilder.Options,
                new DesignTimeTenantProvider()
            );
        }
    }

    /// <summary>
    /// Tenant provider used only during design time operations.
    /// </summary>
    internal sealed class DesignTimeTenantProvider : ITenantProvider
    {
        /// <summary>
        /// Gets the tenant identifier.
        /// </summary>
        public Guid? TenantId => null; // No tenant during migrations/tooling

        /// <summary>
        /// Gets a is host admin.
        /// </summary>
        public bool IsHostAdmin => true; // Always host context for design-time

        /// <summary>
        /// Gets a placeholder tenant slug.
        /// </summary>
        public string TenantSlug => "design-time";

    }
}