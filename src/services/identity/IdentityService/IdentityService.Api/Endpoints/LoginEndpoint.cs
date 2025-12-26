using IdentityService.Application.Dtos;
using IdentityService.Application.Auth;

namespace IdentityService.Api.Endpoints
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/login", async (LoginUserDto dto, LoginUser loginUser) =>
            {
                var result = await loginUser.HandleAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            })
                .WithName("LoginUser")
                .WithTags("Auth");
        }
    }
}
