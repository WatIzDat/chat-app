using Domain.Roles;
using SharedKernel;

namespace Domain.UnitTests.Roles;

public class RoleTests
{
    private static Role CreateDefaultRole()
    {
        return Role.Create(Guid.Empty, "test", []).Value;
    }

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        Role referenceRole = CreateDefaultRole();

        // Act
        Result<Role> result = Role.Create(
            referenceRole.DiscussionId,
            referenceRole.Name,
            referenceRole.Permissions);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        // Arrange
        Role referenceRole = CreateDefaultRole();

        string longerThanMaxLength = string.Empty.PadLeft(Role.NameMaxLength + 1);

        // Act
        Result<Role> result = Role.Create(
            referenceRole.DiscussionId,
            longerThanMaxLength,
            referenceRole.Permissions);

        // Assert
        result.Error.Should().Be(RoleErrors.NameTooLong);
    }

    [Fact]
    public void Create_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionsAreInList()
    {
        // Arrange
        Role referenceRole = CreateDefaultRole();

        List<Permission> hasDuplicatePermissions = [Permission.BanUser, Permission.BanUser];

        // Act
        Result<Role> result = Role.Create(
            referenceRole.DiscussionId,
            referenceRole.Name,
            hasDuplicatePermissions);

        // Assert
        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public void EditName_Should_ReturnSuccess()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        string validName = "a";

        // Act
        Result result = defaultRole.EditName(validName);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void EditName_Should_ChangeName()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        string validName = "a";

        // Act
        defaultRole.EditName(validName);

        // Assert
        defaultRole.Name.Should().Be(validName);
    }
    [Fact]
    public void EditName_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        string longerThanMaxLength = string.Empty.PadLeft(Role.NameMaxLength + 1);

        // Act
        Result result = defaultRole.EditName(longerThanMaxLength);

        // Assert
        result.Error.Should().Be(RoleErrors.NameTooLong);
    }

    [Fact]
    public void AddPermission_Should_ReturnSuccess()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        // Act
        Result result = defaultRole.AddPermission(Permission.KickUser);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void AddPermission_Should_AddPermissionToList()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        // Act
        defaultRole.AddPermission(Permission.KickUser);

        // Assert
        defaultRole.Permissions.Should().Contain(Permission.KickUser);
    }

    [Fact]
    public void AddPermission_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionIsAdded()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        defaultRole.AddPermission(Permission.KickUser);

        // Act
        Result result = defaultRole.AddPermission(Permission.KickUser);

        // Assert
        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public void RemovePermission_Should_RemovePermission()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        defaultRole.AddPermission(Permission.BanUser);

        // Act
        defaultRole.RemovePermission(Permission.BanUser);

        // Assert
        defaultRole.Permissions.Should().NotContain(Permission.BanUser);
    }

    [Fact]
    public void RemovePermission_Should_ReturnPermissionNotFound_WhenPermissionCouldNotBeRemoved()
    {
        // Arrange
        Role defaultRole = CreateDefaultRole();

        Permission permissionNotInList = Permission.MuteUser;

        // Act
        Result result = defaultRole.RemovePermission(permissionNotInList);

        // Assert
        result.Error.Should().Be(RoleErrors.PermissionNotFound);
    }
}
