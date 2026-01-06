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
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<,>));

            // Register Specific Repositories
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();

            // Register Services
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICountryService, CountryService>();

            return services;
        }
    }
}
