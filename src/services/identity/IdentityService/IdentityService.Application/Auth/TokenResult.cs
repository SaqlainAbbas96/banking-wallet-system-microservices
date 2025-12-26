namespace IdentityService.Application.Auth
{
    public record TokenResult(
        string Token,
        DateTime ExpiresAt);
}
