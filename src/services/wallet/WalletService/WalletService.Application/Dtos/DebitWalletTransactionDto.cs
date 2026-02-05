using WalletService.Domain.Enums;

namespace WalletService.Application.Dtos
{
    public record DebitWalletTransactionDto(
        Guid UserId,
        Guid WalletId,
        WalletTransactionType Type,
        decimal Amount,
        string IdempotencyKey);
}
