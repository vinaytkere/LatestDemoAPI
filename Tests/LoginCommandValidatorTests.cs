using Application.Features.Auth.Commands;
using FluentAssertions;

namespace Tests
{
    public class LoginCommandValidatorTests
    {
        [Fact]
        public async Task LoginValidator_ShouldFail_WhenFieldsMissing()
        {
            // Arrange
            var command = new LoginCommand();
            var validator = new LoginCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }
    }
}
