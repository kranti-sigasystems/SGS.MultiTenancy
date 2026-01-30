using Microsoft.Extensions.DependencyInjection;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Services;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.Infra.Repository;

namespace SGS.MultiTenancy.Infa.Extension
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// Adds infrastructure services to the dependency injection container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Updated service collection.</returns>
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            // Register JwtTokenGenerator Services
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}
