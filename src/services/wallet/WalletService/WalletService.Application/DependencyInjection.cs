using Microsoft.Extensions.DependencyInjection;
using WalletService.Application.UseCases;

namespace WalletService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateWalletUseCase>();
            services.AddScoped<GetWalletUseCase>();
            services.AddScoped<CreditWalletTransactionUseCase>();
            services.AddScoped<DebitWalletTransactionUseCase>();

            return services;
        }
    }
}
