using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletService.Application.Interfaces;
using WalletService.Infrastructure.Persistence;
using WalletService.Infrastructure.Repositories;

namespace WalletService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<WalletDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("WalletDb")));

            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();

            return services;
        }
    }
}
