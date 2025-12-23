using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;

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

        public async Task<User> HandleAsync(RegisterUserDto dto)
        {
            // Add Business rule

            var hashedPassword = _passwordHasher.Hash(dto.Password);
            var user = new User(dto.Email, hashedPassword);
            await _userRepository.AddUserAsync(user);
            return user;
        }
    }
}
