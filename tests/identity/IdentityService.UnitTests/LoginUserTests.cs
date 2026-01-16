using FluentAssertions;
using IdentityService.Application.Auth;
using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityService.UnitTests
{
    public class LoginUserTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IPasswordHasher> _hasherMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<ILogger<LoginUser>> _loggerMock = new();

        private LoginUser CreateService() => new(_userRepoMock.Object, _hasherMock.Object, _tokenServiceMock.Object, _loggerMock.Object);

        [Fact]
        public async Task Should_LoginUser_When_ValidCredentials()
        {
            var dto = new LoginUserDto("test@test.com", "Testing123@");
            var user = new User(dto.Email, "hashed");
            _userRepoMock.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
            _hasherMock.Setup(x => x.Verify(dto.Password, user.PasswordHash)).Returns(true);
            _tokenServiceMock.Setup(x => x.GenerateToken(user)).Returns(new TokenResult("jwt123", DateTime.UtcNow.AddMinutes(30)));


            var service = CreateService();
            var result = await service.HandleAsync(dto);

            result.Success.Should().BeTrue();
            result.Data!.AccessToken.Should().Be("jwt123");
            result.Data!.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(30), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Should_ReturnError_When_InvalidCredentials()
        {
            var dto = new LoginUserDto("test@test.com", "Testing123@");
            _userRepoMock.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);

            var service = CreateService();
            var result = await service.HandleAsync(dto);

            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Invalid email or password.");
        }
    }
}
