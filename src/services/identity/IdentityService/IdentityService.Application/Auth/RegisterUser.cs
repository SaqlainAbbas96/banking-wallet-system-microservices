using IdentityService.Application.Common;
using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using System.Text.RegularExpressions;

namespace IdentityService.Application.Auth
{
    public class RegisterUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUser(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<RegisterUserResponse>> HandleAsync(RegisterUserDto dto)
        {
            try
            {
                // Required fields
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return ApiResponse<RegisterUserResponse>.FailureResponse("Email is required.");

                if (string.IsNullOrWhiteSpace(dto.Password))
                    return ApiResponse<RegisterUserResponse>.FailureResponse("Password is required.");

                // Email format validation
                if (!string.IsNullOrWhiteSpace(dto.Email) && !IsValidEmail(dto.Email))
                    return ApiResponse<RegisterUserResponse>.FailureResponse("Email format is invalid.");

                // Password complexity validation
                if (!string.IsNullOrWhiteSpace(dto.Password) && !IsValidPassword(dto.Password))
                    return ApiResponse<RegisterUserResponse>.FailureResponse("Password must be at least 8 characters, include uppercase, lowercase, digit, and special character.");

                // Check if user exists
                var emailExists = await _userRepository.ExistsByEmailAsync(dto.Email);
                if (emailExists)
                    return ApiResponse<RegisterUserResponse>.FailureResponse("Email already exists.");

                var hashedPassword = _passwordHasher.Hash(dto.Password!);

                var user = new User(dto.Email, hashedPassword);

                await _userRepository.AddUserAsync(user);

                var response = new RegisterUserResponse(
                    user.Id,
                    user.Email,
                    user.CreatedAt
                );

                return ApiResponse<RegisterUserResponse>.SuccessResponse(response, "User registered successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<RegisterUserResponse>.FailureResponse("An unexpected error occurred.", ex.Message);
            }
        }

        // -----------------------
        // Helpers
        // -----------------------
        private bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private bool IsValidPassword(string password)
        {
            // Minimum 8 chars, at least 1 uppercase, 1 lowercase, 1 digit, 1 special char
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
