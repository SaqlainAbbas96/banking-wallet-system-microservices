using WalletService.Api.Contracts;
using WalletService.Api.Contracts.Requests;
using WalletService.Api.Infrastructure.Auth;
using WalletService.Application.Dtos;
using WalletService.Application.UseCases;
using WalletService.Domain.Enums;

namespace WalletService.Api.Endpoints
{
    public static class WalletEndpoint
    {
        public static void MapWalletEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost(Routes.Wallet.Create, CreateWallet)
                .WithName("CreateWallet")
                .WithTags("Wallet")
                .RequireAuthorization();

            app.MapGet(Routes.Wallet.Get, GetWallets)
                .WithName("GetWallets")
                .WithTags("Wallet")
                .RequireAuthorization();

            app.MapPost(Routes.Wallet.Credit, CreditWallet)
                .WithName("CreditWallet")
                .WithTags("Wallet")
                .RequireAuthorization();

            app.MapPost(Routes.Wallet.Debit, DebitWallet)
                .WithName("DebitWallet")
                .WithTags("Wallet")
                .RequireAuthorization();
        }

        private static async Task<IResult> CreateWallet(
            CreateWalletRequest request,
            CreateWalletUseCase createWallet,
            HttpContext httpContext,
            CancellationToken ct)
        {
            var userId = httpContext.GetRequiredUserId();

            var dto = new CreateWalletDto(
                userId,
                request.Currency
            );

            var result = await createWallet.ExecuteAsync(dto, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        }

        private static async Task<IResult> GetWallets(
            GetWalletUseCase useCase,
            HttpContext httpContext,
            CancellationToken ct)
        {
            var userId = httpContext.GetRequiredUserId();

            var dto = new GetWalletDto(
                userId
            );

            var result = await useCase.ExecuteAsync(dto, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        }

        private static async Task<IResult> CreditWallet(
            CreditWalletTransactionRequest request,
            CreditWalletTransactionUseCase useCase,
            HttpContext httpContext,
            CancellationToken ct)
        {
            var userId = httpContext.GetRequiredUserId();

            var dto = new CreditWalletTransactionDto(
                userId,
                request.WalletId,
                WalletTransactionType.Credit,
                request.Amount,
                request.IdempotencyKey
            );

            var result = await useCase.ExecuteAsync(dto, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        }

        private static async Task<IResult> DebitWallet(
            DebitWalletTransactionRequest request,
            DebitWalletTransactionUseCase useCase,
            HttpContext httpContext,
            CancellationToken ct)
        {
            var userId = httpContext.GetRequiredUserId();

            var dto = new DebitWalletTransactionDto(
                userId,
                request.WalletId,
                WalletTransactionType.Debit,
                request.Amount,
                request.IdempotencyKey
            );

            var result = await useCase.ExecuteAsync(dto, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        }
    }
}