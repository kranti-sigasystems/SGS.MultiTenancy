
using Microsoft.Extensions.DependencyInjection;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Interfaces.Repositories;
using SGS.MultiTenancy.Infra.Repositery;
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
            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepositery, UserRepository>();

            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
         
            // Register JwtTokenGenerator Services
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}
