using IdentityService.Application.Common;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces
{
    public interface ITokenService
    {
        TokenResult GenerateToken(User user);
    }
}
