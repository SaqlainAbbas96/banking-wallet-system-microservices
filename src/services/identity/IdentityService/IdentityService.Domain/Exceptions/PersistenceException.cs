namespace IdentityService.Domain.Exceptions
{
    public sealed class PersistenceException : DomainException
    {
        public PersistenceException(string message)
            : base(message)
        {
        }
    }
}
