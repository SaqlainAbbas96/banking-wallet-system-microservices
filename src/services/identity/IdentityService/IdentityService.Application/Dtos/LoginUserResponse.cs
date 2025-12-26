namespace IdentityService.Application.Dtos;

public record LoginUserResponse(
    string AccessToken,
    DateTime ExpiresAt
);