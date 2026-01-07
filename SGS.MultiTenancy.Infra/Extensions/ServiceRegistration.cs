using Microsoft.Extensions.Configuration;
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
            services.AddHttpContextAccessor(); // Needed if you use IHttpContextAccessor

            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepositery, UserRepositery>();

            services.AddScoped<ITenantProvider, TenantProvider>();

            // Register JwtTokenGenerator Services
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
