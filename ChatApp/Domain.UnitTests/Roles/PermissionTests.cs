using Domain.Roles;
using SharedKernel;

namespace Domain.UnitTests.Roles;

public class PermissionTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        Result<Permission> result = Permission.Create("user:ban");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnNotAllowed_WhenAllowedPermissionsDoesNotContainPermission()
    {
        Result<Permission> result = Permission.Create("not allowed");

        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }
}
