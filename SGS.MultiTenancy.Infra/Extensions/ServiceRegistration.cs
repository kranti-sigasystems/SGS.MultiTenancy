using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Services;
using SGS.MultiTenancy.Core.Services;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
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
            services.AddHttpContextAccessor(); // Needed if you use IHttpContextAccessor

            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register Specific Repositories
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();

            // Register Services
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ITenantProvider, TenantProvider>();

            // Register JwtTokenGenerator Services
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
