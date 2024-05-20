using Application.Roles.RemovePermission;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class RemovePermissionCommandTests : BaseRoleTest<RemovePermissionCommand>
{
    private static readonly Permission TestPermission = Permission.KickUser;
    
    protected override List<Permission> CreateDefaultPermissionsList()
    {
        return [TestPermission];
    }

    private readonly RemovePermissionCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    protected override void ConfigureMocks(Role role, RemovePermissionCommand command, Action? overrides = null)
    {
        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        base.ConfigureMocks(role, command, overrides);
    }

    public RemovePermissionCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new RemovePermissionCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        Role role = CreateDefaultRole();

        RemovePermissionCommand command = new(role.Id, TestPermission.Value);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_RemovePermission()
    {
        // Arrange
        Role role = CreateDefaultRole();

        RemovePermissionCommand command = new(role.Id, TestPermission.Value);

        ConfigureMocks(role, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        role.Permissions.Should().NotContain(TestPermission);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        Role role = CreateDefaultRole();

        RemovePermissionCommand command = new(role.Id, TestPermission.Value);

        ConfigureMocks(role, command, overrides: () =>
        {
            roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).ReturnsNull();
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnPermissionNotAllowed_WhenPermissionIsNotInListOfAllowedPermissions()
    {
        // Arrange
        Role role = CreateDefaultRole();

        string invalidPermission = "";

        RemovePermissionCommand command = new(role.Id, invalidPermission);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnPermissionNotFound_WhenPermissionIsNotInPermissionsList()
    {
        // Arrange
        Role role = CreateDefaultRole();

        string permissionNotInPermissionsList = Permission.BanUser.Value;

        RemovePermissionCommand command = new(role.Id, permissionNotInPermissionsList);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.PermissionNotFound);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryUpdate()
    {
        // Arrange
        Role role = CreateDefaultRole();

        RemovePermissionCommand command = new(role.Id, TestPermission.Value);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        roleRepositoryMock
            .Received(1)
            .Update(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
