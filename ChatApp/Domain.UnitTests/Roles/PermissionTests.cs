using Domain.Roles;
using SharedKernel;

namespace Domain.UnitTests.Roles;

public class PermissionTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        string validValue = "user:ban";

        // Act
        Result<Permission> result = Permission.Create(validValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnNotAllowed_WhenAllowedPermissionsDoesNotContainPermission()
    {
        // Arrange
        string invalidValue = string.Empty;

        // Act
        Result<Permission> result = Permission.Create(invalidValue);

        // Assert
        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }
}
