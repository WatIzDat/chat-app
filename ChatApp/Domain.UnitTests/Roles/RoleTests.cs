using Domain.Roles;
using SharedKernel;

namespace Domain.UnitTests.Roles;

public class RoleTests
{
    private static readonly List<Permission> Permissions = [Permission.BanUser, Permission.DeleteMessage];

    private static readonly Role Role = Role.Create(Guid.NewGuid(), "test", Permissions).Value;

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        Result<Role> result = Role.Create(Role.DiscussionId, Role.Name, Permissions);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        Result<Role> result = Role.Create(Role.DiscussionId, "thisnameiswaytoolongaaaaaaaaaaaaaaa", Permissions);

        result.Error.Should().Be(RoleErrors.NameTooLong);
    }

    [Fact]
    public void Create_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionsAreInList()
    {
        Result<Role> result = Role.Create(
            Role.DiscussionId,
            Role.Name,
            [Permission.BanUser, Permission.BanUser]);

        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public void EditName_Should_ReturnSuccess_And_ChangeName()
    {
        Result result = Role.EditName("hello");

        result.IsSuccess.Should().BeTrue();

        Role.Name.Should().Be("hello");
    }

    [Fact]
    public void EditName_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        Result result = Role.EditName("thisnameiswaytoolongaaaaaaaaaaaaaaa");

        result.Error.Should().Be(RoleErrors.NameTooLong);
    }

    [Fact]
    public void AddPermission_Should_ReturnSuccess_And_AddPermissionToList()
    {
        Result result = Role.AddPermission(Permission.KickUser);

        result.IsSuccess.Should().BeTrue();

        Role.Permissions.Should().Contain(Permission.KickUser);
    }

    [Fact]
    public void AddPermission_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionIsAdded()
    {
        Role.AddPermission(Permission.KickUser);

        Result result = Role.AddPermission(Permission.KickUser);

        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public void RemovePermission_Should_RemovePermission()
    {
        Role.RemovePermission(Permission.BanUser);

        Role.Permissions.Should().NotContain(Permission.BanUser);
    }

    [Fact]
    public void RemovePermission_Should_ReturnPermissionNotFound_WhenPermissionCouldNotBeRemoved()
    {
        Result result = Role.RemovePermission(Permission.MuteUser);

        result.Error.Should().Be(RoleErrors.PermissionNotFound);
    }
}
