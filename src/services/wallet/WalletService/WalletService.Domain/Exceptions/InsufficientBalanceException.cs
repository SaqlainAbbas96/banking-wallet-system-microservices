namespace WalletService.Domain.Exceptions
{
    public sealed class InsufficientBalanceException : DomainException
    {
        public InsufficientBalanceException() 
            : base("Insufficient balance") 
        {
        }
    }
}
