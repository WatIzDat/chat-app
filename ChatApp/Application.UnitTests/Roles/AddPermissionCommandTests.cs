using Application.Roles.AddPermission;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class AddPermissionCommandTests : BaseRoleTest<AddPermissionCommand>
{
    private readonly AddPermissionCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    protected override void ConfigureMocks(Role role, AddPermissionCommand command, Action? overrides = null)
    {
        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        base.ConfigureMocks(role, command, overrides);
    }

    public AddPermissionCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new AddPermissionCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        Role role = CreateDefaultRole();

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_AddPermission()
    {
        // Arrange
        Role role = CreateDefaultRole();

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

        ConfigureMocks(role, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        role.Permissions.Should().Contain(Permission.KickUser);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        Role role = CreateDefaultRole();

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

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

        AddPermissionCommand command = new(role.Id, invalidPermission);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionIsAdded()
    {
        // Arrange
        Role role = CreateDefaultRole();

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

        ConfigureMocks(role, command);

        await commandHandler.Handle(command, default);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryUpdate()
    {
        // Arrange
        Role role = CreateDefaultRole();

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        roleRepositoryMock
            .Received(1)
            .Update(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
