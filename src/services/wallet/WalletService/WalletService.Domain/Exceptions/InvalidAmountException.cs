namespace WalletService.Domain.Exceptions
{
    public sealed class InvalidAmountException : DomainException
    {
        public InvalidAmountException() 
            : base("Invalid amount") 
        {
        }
    }
}
