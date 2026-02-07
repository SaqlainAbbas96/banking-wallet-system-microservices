namespace IdentityService.Application.Dtos;

public class LoginUserResponse
{
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}