using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SGS.MultiTenancy.Core.Application.Interfaces;

namespace SGS.MultiTenancy.Infra.DataContext
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8,0,44)) // 👈 IMPORTANT
            );

            return new AppDbContext(
                optionsBuilder.Options,
                new DesignTimeTenantProvider()
            );
        }
    }

    internal sealed class DesignTimeTenantProvider : ITenantProvider
    {
        public Guid TenantId => Guid.Empty;
    }
}
