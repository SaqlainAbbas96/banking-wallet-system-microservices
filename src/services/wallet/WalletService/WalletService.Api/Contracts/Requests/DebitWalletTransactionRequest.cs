using WalletService.Domain.Enums;

namespace WalletService.Api.Contracts.Requests
{
    public sealed record DebitWalletTransactionRequest(
        Guid UserId,
        Guid WalletId,
        WalletTransactionType Type,
        decimal Amount,
        string IdempotencyKey);
}
