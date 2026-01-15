using Microsoft.Extensions.DependencyInjection;
using SGS.MultiTenancy.Core.Services;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.Core.Extension
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// Adds infrastructure services to the dependency injection container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Updated service collection.</returns>
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IPasswordHasherService,PasswordHasherService>();


            services.AddScoped<IPermissionService,PermissionService>();
            return services;
        }
    }
}
