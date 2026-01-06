using Microsoft.Extensions.DependencyInjection;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Infra.Repositery;

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
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
