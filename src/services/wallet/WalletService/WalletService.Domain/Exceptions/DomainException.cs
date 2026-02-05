namespace WalletService.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        //public string Message { get; }

        protected DomainException(string message)
            : base(message)
        {
        }
    }
}
