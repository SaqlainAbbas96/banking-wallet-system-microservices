namespace IdentityService.Application.Common
{
    public sealed record TokenResult(
        string Token,
        DateTime ExpiresAt);
}
