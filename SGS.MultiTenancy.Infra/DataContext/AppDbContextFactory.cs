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

            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8,0,44))
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
        /// Returns an empty tenant identifier to disable tenant filtering.
        /// </summary>
        public Guid TenantId => Guid.Empty;
    }
}