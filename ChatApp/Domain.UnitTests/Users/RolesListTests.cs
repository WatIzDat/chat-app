using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class RolesListTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        Guid testGuid = Guid.Empty;

        List<Guid> roles = [testGuid];

        // Act
        Result<RolesList> result = RolesList.Create(roles);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnDuplicateRoles_WhenDuplicateGuidsAreInList()
    {
        // Arrange
        Guid testGuid = Guid.Empty;

        List<Guid> roles = [testGuid, testGuid];

        // Act
        Result<RolesList> result = RolesList.Create(roles);

        // Assert
        result.Error.Should().Be(RolesListErrors.DuplicateRoles);
    }
}
