using IdentityService.Application.Common;
using IdentityService.Application.Dtos;
using IdentityService.Application.Helpers.Validation;
using IdentityService.Application.Interfaces;

namespace IdentityService.Application.Auth
{
    public class LoginUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService; 

        public LoginUser(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<LoginUserResponse>> HandleAsync(LoginUserDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return ApiResponse<LoginUserResponse>.FailureResponse("Email is required.");

                if (string.IsNullOrWhiteSpace(dto.Password))
                    return ApiResponse<LoginUserResponse>.FailureResponse("Password is required.");

                if (!CredentialValidator.IsValidEmail(dto.Email))
                    return ApiResponse<LoginUserResponse>.FailureResponse("Email format is invalid.");

                if (!CredentialValidator.IsValidPassword(dto.Password))
                    return ApiResponse<LoginUserResponse>.FailureResponse(
                        "Password must be at least 8 characters, include uppercase, lowercase, digit, and special character.");

                var user = await _userRepository.GetByEmailAsync(dto.Email);
                if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
                    return ApiResponse<LoginUserResponse>.FailureResponse("Invalid email or password.");

                var tokenResult = _tokenService.GenerateToken(user);

                var response = new LoginUserResponse(
                    tokenResult.Token,
                    tokenResult.ExpiresAt
                );

                return ApiResponse<LoginUserResponse>.SuccessResponse(response, "Login successful.");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginUserResponse>.FailureResponse("An unexpected error occurred.");
            }
        }
    }
}
