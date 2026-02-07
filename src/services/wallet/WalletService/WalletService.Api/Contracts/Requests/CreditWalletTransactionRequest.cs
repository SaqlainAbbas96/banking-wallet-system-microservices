using WalletService.Domain.Enums;

namespace WalletService.Api.Contracts.Requests
{
    public sealed record CreditWalletTransactionRequest(
        Guid UserId,
        Guid WalletId,
        WalletTransactionType Type,
        decimal Amount,
        string IdempotencyKey); 
}
