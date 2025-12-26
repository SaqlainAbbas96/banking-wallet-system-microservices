namespace IdentityService.Application.Dtos;

public record RegisterUserDto(
    string Email,
    string Password
);