using System.ComponentModel.DataAnnotations;
using WalletService.Domain.Exceptions;

namespace WalletService.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; private set; }
        public string Currency { get; set; } = "GBP";
        
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Wallet(Guid userId, string currency)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Currency = currency;
            Balance = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ApplyCredit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidAmountException();

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ApplyDebit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidAmountException();

            if (Balance < amount)
                throw new InsufficientBalanceException();

            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
