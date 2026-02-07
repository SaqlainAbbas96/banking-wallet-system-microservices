using WalletService.Domain.Entities;

namespace WalletService.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetByIdAsync(Guid walletId, CancellationToken ct);
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task AddAsync(Wallet wallet, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
