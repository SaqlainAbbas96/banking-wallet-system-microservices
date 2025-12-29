using IdentityService.Api.Contracts;
using IdentityService.Application.Auth;
using IdentityService.Application.Dtos;

namespace IdentityService.Api.Endpoints
{
    public static class AuthEndpoint
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost(Routes.Auth.Register, Register)
                .WithName("RegisterUser")
                .WithTags("Identity/Auth");

            app.MapPost(Routes.Auth.Login, Login)
                .WithName("LoginUser")
                .WithTags("Identity/Auth");
        }

        private static async Task<IResult> Register(
            RegisterUserDto dto,
            RegisterUser useCase)
        {
            var result = await useCase.HandleAsync(dto);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> Login(
            LoginUserDto dto,
            LoginUser useCase)
        {
            var result = await useCase.HandleAsync(dto);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }
    }
}
