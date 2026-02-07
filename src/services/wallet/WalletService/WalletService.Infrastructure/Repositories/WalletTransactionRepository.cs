using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Repositories
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly WalletDbContext _db;
        public WalletTransactionRepository(WalletDbContext db)
        {
            _db = db;
        }

        //public Task<bool> ExistsAsync(Guid walletId, string idempotencyKey, CancellationToken ct)
        //    => _db.WalletTransactions.AnyAsync(
        //        x => x.WalletId == walletId &&
        //             x.IdempotencyKey == idempotencyKey,
        //        ct);

        public async Task AddAsync(WalletTransaction walletTransaction, CancellationToken ct)
            => await _db.WalletTransactions.AddAsync(walletTransaction, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<WalletTransaction?> GetByIdempotencyKeyAsync(
            Guid walletId,
            string idempotencyKey,
            CancellationToken ct)
        {
            return await _db.WalletTransactions
                .AsNoTracking()
                .Where(t =>
                    t.WalletId == walletId &&
                    t.IdempotencyKey == idempotencyKey)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync(ct);
        }

    }
}
