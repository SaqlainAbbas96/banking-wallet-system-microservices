using WalletService.Domain.Enums;

namespace WalletService.Application.Dtos
{
    public class WalletTransactionResponse
    {
        public Guid WalletId { get; set; }
        public Guid TransactionId { get; set; }
        public decimal Balance { get; set; }
        public WalletTransactionType Type { get; set; }
    }
}
