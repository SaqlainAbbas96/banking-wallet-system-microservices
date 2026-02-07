using WalletService.Domain.Entities;

namespace WalletService.Application.Interfaces
{
    public interface IWalletTransactionRepository
    {
        //Task<bool> ExistsAsync(Guid walletId, string idempotencyKey, CancellationToken ct);
        Task AddAsync(WalletTransaction walletTransaction, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
        Task<WalletTransaction?> GetByIdempotencyKeyAsync(
            Guid walletId,
            string idempotencyKey,
            CancellationToken ct);
    }
}
