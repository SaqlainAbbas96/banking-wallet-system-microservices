using IdentityService.Api.Contracts;
using IdentityService.Api.Contracts.Requests;
using IdentityService.Application.Dtos;
using IdentityService.Application.UseCases;

namespace IdentityService.Api.Endpoints
{
    public static class AuthenticationEndpoint
    {
        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost(Routes.Authentication.Register, Register)
                .WithName("RegisterUser")
                .WithTags("Identity/Auth");

            app.MapPost(Routes.Authentication.Login, Login)
                .WithName("LoginUser")
                .WithTags("Identity/Auth");
        }

        private static async Task<IResult> Register(
            RegisterUserRequest request,
            RegisterUserUseCase registerUser)
        {
            var dto = new RegisterUserDto(
                request.Email,
                request.Password
            );

            var result = await registerUser.ExecuteAsync(dto);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        }

        private static async Task<IResult> Login(
            LoginUserRequest request,
            LoginUserUseCase loginUser)
        {
            var dto = new LoginUserDto(
                request.Email,
                request.Password
            );

            var result = await loginUser.ExecuteAsync(dto);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        }
    }
}
