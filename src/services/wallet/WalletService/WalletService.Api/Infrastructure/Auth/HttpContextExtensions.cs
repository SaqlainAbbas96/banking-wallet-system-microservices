using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WalletService.Api.Infrastructure.Auth
{
    public static class HttpContextExtensions
    {
        public static Guid GetRequiredUserId(this HttpContext context)
        {
            var claim = context.User.FindFirst(ClaimTypes.NameIdentifier)
                ?? context.User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (claim == null || !Guid.TryParse(claim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid or missing user id");

            return userId;
        }
    }
}
