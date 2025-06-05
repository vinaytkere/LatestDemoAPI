using Application.Features.Auth.Commands;
using FluentAssertions;

namespace Tests
{
    public class RegisterUserCommandValidatorTests
    {
        [Fact]
        public async Task RegisterValidator_ShouldFail_WhenFieldsMissing()
        {
            // Arrange
            var command = new RegisterUserCommand();
            var validator = new RegisterUserCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }
    }
}
