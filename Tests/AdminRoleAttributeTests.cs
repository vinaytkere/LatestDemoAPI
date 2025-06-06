using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;
using System.Reflection;

namespace Tests
{
    public class AdminRoleAttributeTests
    {
        [Fact]
        public void Create_ShouldRequireAdminRole()
        {
            // Act
            var method = typeof(AddressController).GetMethod("Create")!;
            var attr = method.GetCustomAttribute<AuthorizeAttribute>();

            // Assert
            attr.Should().NotBeNull();
            attr!.Roles.Should().Be("Admin");
        }

        [Fact]
        public void Update_ShouldRequireAdminRole()
        {
            var method = typeof(AddressController).GetMethod("Update")!;
            var attr = method.GetCustomAttribute<AuthorizeAttribute>();
            attr.Should().NotBeNull();
            attr!.Roles.Should().Be("Admin");
        }

        [Fact]
        public void Delete_ShouldRequireAdminRole()
        {
            var method = typeof(AddressController).GetMethod("Delete")!;
            var attr = method.GetCustomAttribute<AuthorizeAttribute>();
            attr.Should().NotBeNull();
            attr!.Roles.Should().Be("Admin");
        }
    }
}
