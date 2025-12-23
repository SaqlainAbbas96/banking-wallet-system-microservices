using IdentityService.Application;
using IdentityService.Application.Auth;

namespace IdentityService.Api.Endpoints
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/register", async (RegisterUserDto dto, RegisterUser registerUser) =>
            {
                var user = await registerUser.HandleAsync(dto);
                return Results.Ok(new { user.Id, user.Email });
            })
                .WithName("RegisterUser")
                .WithTags("Auth");
        }
    }
}
