using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;

namespace Tests
{
    public class AuthorizationAttributeTests
    {
        [Fact]
        public void AddressController_ShouldBeDecoratedWithAuthorize()
        {
            // Act
            var hasAttr = typeof(AddressController).GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

            // Assert
            hasAttr.Should().BeTrue();
        }
    }
}
