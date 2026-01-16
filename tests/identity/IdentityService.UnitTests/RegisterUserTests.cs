using FluentAssertions;
using IdentityService.Application.Auth;
using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityService.UnitTests;

public class RegisterUserTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IPasswordHasher> _hasherMock = new();
    private readonly Mock<ILogger<RegisterUser>> _loggerMock = new();

    private RegisterUser CreateService() => new(_userRepoMock.Object, _hasherMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Should_RegisterUser_When_ValidInput()
    {
        // Arrange
        var dto = new RegisterUserDto("test@test.com", "Testing123@");

        // Mock the correct method
        _userRepoMock.Setup(x => x.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
        _hasherMock.Setup(x => x.Hash(dto.Password)).Returns("hashed123");

        var service = CreateService();

        // Act
        var result = await service.HandleAsync(dto);

        // Assert
        result.Success.Should().BeTrue();
        result.Data!.Email.Should().Be(dto.Email);
        _userRepoMock.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnError_When_EmailExists()
    {
        var dto = new RegisterUserDto("test@test.com", "Testing123@");
        _userRepoMock.Setup(x => x.ExistsByEmailAsync(dto.Email))
                         .ReturnsAsync(true);

        var service = CreateService();
        var result = await service.HandleAsync(dto);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Email already exists.");
        _userRepoMock.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Never);
    }
}
