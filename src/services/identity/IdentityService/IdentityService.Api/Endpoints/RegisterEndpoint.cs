using IdentityService.Application.Dtos;
using IdentityService.Application.Auth;

namespace IdentityService.Api.Endpoints
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/register", async (RegisterUserDto dto, RegisterUser registerUser) =>
            {
                var result = await registerUser.HandleAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            })
                .WithName("RegisterUser")
                .WithTags("Auth");
        }
    }
}
