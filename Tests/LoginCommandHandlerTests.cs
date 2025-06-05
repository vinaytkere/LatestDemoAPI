using Application.Features.Auth.Commands;
using Domain;
using Moq;
using FluentAssertions;
using Bogus;
using static Application.Interfaces.IRepository;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using System.Linq.Expressions;

namespace Tests
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _repoMock = new();
        private readonly Mock<IPasswordHasher<User>> _hasherMock = new();
        private readonly Mock<IJwtTokenGenerator> _tokenMock = new();

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            var faker = new Faker<LoginCommand>()
                .RuleFor(x => x.Username, f => f.Internet.UserName())
                .RuleFor(x => x.Password, f => f.Internet.Password());

            var command = faker.Generate();

            var user = new User { Id = Guid.NewGuid(), UserName = command.Username, PasswordHash = "hashed" };

            _repoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            _hasherMock.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, command.Password))
                .Returns(PasswordVerificationResult.Success);

            _tokenMock.Setup(t => t.GenerateToken(user)).Returns("jwt-token");

            var handler = new LoginCommandHandler(_repoMock.Object, _hasherMock.Object, _tokenMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("jwt-token");
        }

        [Fact]
        public async Task Login_ShouldFail_WhenInvalidCredentials()
        {
            // Arrange
            var command = new LoginCommand { Username = "bad", Password = "bad" };
            _repoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            var handler = new LoginCommandHandler(_repoMock.Object, _hasherMock.Object, _tokenMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
