using WalletService.Domain.Enums;

namespace WalletService.Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; private set; }
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public string Type { get; private set; }
        public string IdempotencyKey { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private WalletTransaction() { }

        public static WalletTransaction Create(
            Guid walletId,
            WalletTransactionType type,
            decimal amount,
            string idempotencyKey)
        {
            return new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                Amount = amount,
                Type = type.ToString().ToUpperInvariant(),
                IdempotencyKey = idempotencyKey,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
