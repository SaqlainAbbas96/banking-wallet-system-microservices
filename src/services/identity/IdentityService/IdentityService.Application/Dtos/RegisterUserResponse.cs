namespace IdentityService.Application.Dtos
{
    public record RegisterUserResponse(
        Guid Id,
        string Email,
        DateTime CreatedAt
    );
}
