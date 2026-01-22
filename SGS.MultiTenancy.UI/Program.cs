using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Extension;
using SGS.MultiTenancy.Infa.Extension;
using SGS.MultiTenancy.Infra.DataContext;
using SGS.MultiTenancy.UI.Attribute;
using SGS.MultiTenancy.UI.Middlewares;
using System.Text;

namespace SGS.MultiTenancy.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ),
                b => b.MigrationsAssembly("SGS.MultiTenancy.Infra")
            ));

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission",
                    policy => policy.AddRequirements(new PermissionRequirement()));
            });
            builder.Services.AddCoreDependencies();
            builder.Services.AddInfrastructureDependencies();

            JwtOptions? jwtOptions = builder.Configuration
                   .GetSection("ApiSettings:JwtOptions")
                   .Get<JwtOptions>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/PermissionNotFound";
                options.Cookie.Name = "SGS_MultiTenancyAuth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(jwtOptions!.ExpiryMinutes);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        var returnUrl = context.Request.Path + context.Request.QueryString;
                        //var redirectUrl = "/Tenant/Discovery";
                        var host = context.Request.Host.Host;
                        if (!host.Contains("."))
                        {
                            context.Response.Redirect("/Tenant/Discovery");
                        }
                        else
                        {
                            context.Response.Redirect("/Auth/Login");
                        }
                        //context.Response.Redirect(redirectUrl);
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.Redirect("/Auth/PermissionNotFound");
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions!.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret)
                    )
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["SGS_AuthToken"];
                        return Task.CompletedTask;
                    }
                };
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<TenantMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "tenantDiscovery",
                pattern: "/",
                defaults: new { controller = "Tenant", action = "Discovery" });
            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}