using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class RolesListTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        List<Guid> roles = [Guid.NewGuid(), Guid.NewGuid()];
        DiscussionsList discussions = DiscussionsList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;

        // Act
        Result<RolesList> result = RolesList.Create(roles);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnDuplicateRoles_WhenDuplicateGuidsAreInList()
    {
        // Arrange
        Guid roleId = Guid.NewGuid();

        List<Guid> roles = [roleId, roleId];
        DiscussionsList discussions = DiscussionsList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;

        // Act
        Result<RolesList> result = RolesList.Create(roles);

        // Assert
        result.Error.Should().Be(RolesListErrors.DuplicateRoles);
    }
}
