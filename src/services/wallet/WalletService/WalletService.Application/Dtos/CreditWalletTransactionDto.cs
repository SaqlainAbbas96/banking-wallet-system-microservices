using WalletService.Domain.Enums;

namespace WalletService.Application.Dtos
{
    public record CreditWalletTransactionDto(
        Guid UserId,
        Guid WalletId,
        WalletTransactionType Type,
        decimal Amount,
        string IdempotencyKey
    );
}
