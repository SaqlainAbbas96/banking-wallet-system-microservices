using IdentityService.Application.Common;
using IdentityService.Application.Dtos;
using IdentityService.Application.Helpers.Validation;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RegisterUserUseCase> _logger;
        public RegisterUserUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ILogger<RegisterUserUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result<RegisterUserResponse>> ExecuteAsync(RegisterUserDto dto)
        {
            _logger.LogInformation("Register user request received for email {Email}", dto.Email);

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result<RegisterUserResponse>.Failure("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return Result<RegisterUserResponse>.Failure("Password is required.");

            if (!CredentialValidator.IsValidEmail(dto.Email))
                return Result<RegisterUserResponse>.Failure("Email format is invalid.");

            if (!CredentialValidator.IsValidPassword(dto.Password))
                return Result<RegisterUserResponse>.Failure(
                    "Password must be at least 8 characters, include uppercase, lowercase, digit, and special character.");

            var emailExists = await _userRepository.ExistsByEmailAsync(dto.Email);
            if (emailExists)
            {
                _logger.LogWarning("Registration failed. Email already exists: {Email}", dto.Email);
                return Result<RegisterUserResponse>.Failure("Email already exists.");
            }

            var hashedPassword = _passwordHasher.Hash(dto.Password);
            var user = new User(dto.Email, hashedPassword);

            try
            {
                await _userRepository.AddUserAsync(user);
            }
            catch (PersistenceException ex)
            {
                _logger.LogError(ex, "Failed to register user {Email}", dto.Email);
                return Result<RegisterUserResponse>.Failure("Unable to register user at this time.");
            }

            _logger.LogInformation("User registered successfully. UserId: {UserId}", user.Id);

            return Result<RegisterUserResponse>.Success(new RegisterUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            });
        }
    }
}
