namespace IdentityService.Application.Dtos;

public class RegisterUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}