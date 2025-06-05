using Application.Features.Auth.Commands;
using Domain;
using Moq;
using FluentAssertions;

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
    }
}
