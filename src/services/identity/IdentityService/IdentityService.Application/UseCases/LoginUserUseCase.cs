using IdentityService.Application.Common;
using IdentityService.Application.Dtos;
using IdentityService.Application.Helpers.Validation;
using IdentityService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUserUseCase> _logger;

        public LoginUserUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            ILogger<LoginUserUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<LoginUserResponse>> ExecuteAsync(LoginUserDto dto)
        {
            _logger.LogInformation("Login attempt received for email {Email}", dto.Email);

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result<LoginUserResponse>.Failure("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return Result<LoginUserResponse>.Failure("Password is required.");

            if (!CredentialValidator.IsValidEmail(dto.Email))
                return Result<LoginUserResponse>.Failure("Email format is invalid.");

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed. Invalid credentials for email {Email}", dto.Email);
                return Result<LoginUserResponse>.Failure("Invalid email or password.");
            }

            var tokenResult = _tokenService.GenerateToken(user);

            _logger.LogInformation("Login successful. UserId: {UserId}", user.Id);

            return Result<LoginUserResponse>.Success(new LoginUserResponse
            {
                AccessToken = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt
            });
        }
    }
}
