using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _db;
        public WalletRepository(WalletDbContext db)
        {
            _db = db;
        }

        public Task<Wallet?> GetByIdAsync(Guid walletId, CancellationToken ct)
        {
            return _db.Wallets.FirstOrDefaultAsync(x => x.Id == walletId, ct);
        }

        public Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return _db.Wallets.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        public async Task AddAsync(Wallet wallet, CancellationToken ct)
            => await _db.Wallets.AddAsync(wallet, ct);


        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
