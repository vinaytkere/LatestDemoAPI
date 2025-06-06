using Application.Features.Auth.Commands;
using Domain;
using Moq;
using FluentAssertions;
using System.Linq.Expressions;
using static Application.Interfaces.IRepository;
using Microsoft.AspNetCore.Identity;

namespace Tests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _repoMock = new();
        private readonly Mock<IPasswordHasher<User>> _hasherMock = new();

        [Fact]
        public async Task Register_ShouldCreateUser_WhenDataValid()
        {
            // Arrange
            var command = new RegisterUserCommand { Username = "new", Password = "pass" };
            User? added = null;
            _hasherMock.Setup(h => h.HashPassword(It.IsAny<User>(), command.Password))
                .Returns("hashed");
            _repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => added = u)
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var handler = new RegisterUserCommandHandler(_repoMock.Object, _hasherMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            added.Should().NotBeNull();
            added!.UserName.Should().Be(command.Username);
            added.PasswordHash.Should().Be("hashed");
            added.Role.Should().Be("User");
        }

        [Fact]
        public async Task Register_ShouldFail_WhenUserExists()
        {
            // Arrange
            var command = new RegisterUserCommand { Username = "existing", Password = "pass123" };
            _repoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { new User() });

            var handler = new RegisterUserCommandHandler(_repoMock.Object, _hasherMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("User already exists");
            _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}