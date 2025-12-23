using IdentityService.Application.Auth;
using IdentityService.Application.Interfaces;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Repositories;
using IdentityService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("IdentityDb")));

            // Repositories & Infrastructure
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();

            // Application services (use cases)
            services.AddScoped<RegisterUser>();

            return services;
        }
    }
}
