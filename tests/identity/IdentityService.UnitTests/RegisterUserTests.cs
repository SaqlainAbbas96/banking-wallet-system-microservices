using FluentAssertions;
using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Application.UseCases;
using IdentityService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityService.UnitTests;

public class RegisterUserTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IPasswordHasher> _hasherMock = new();
    private readonly Mock<ILogger<RegisterUserUseCase>> _loggerMock = new();

    private RegisterUserUseCase CreateService() => new(_userRepoMock.Object, _hasherMock.Object, _loggerMock.Object);

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
        var result = await service.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Email.Should().Be(dto.Email);
        _userRepoMock.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnError_When_EmailExists()
    {
        var dto = new RegisterUserDto("test@test.com", "Testing123@");
        _userRepoMock.Setup(x => x.ExistsByEmailAsync(dto.Email))
                         .ReturnsAsync(true);

        var service = CreateService();
        var result = await service.ExecuteAsync(dto);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Email already exists.");
        _userRepoMock.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Never);
    }
}
