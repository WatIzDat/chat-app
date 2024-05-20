using Application.Roles.CreateRole;
using Domain.Discussions;
using Domain.Roles;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class CreateRoleCommandTests : BaseRoleTest<CreateRoleCommand>
{
    private static readonly CreateRoleCommand Command = new(Guid.Empty, "test", []);

    private readonly CreateRoleCommandHandler commandHandler;

    private readonly IRoleRepository roleRepositoryMock;
    private readonly IDiscussionRepository discussionRepositoryMock;

    protected override void ConfigureMocks(CreateRoleCommand command, Action? overrides = null)
    {
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);
        roleRepositoryMock
            .DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId))
            .Returns(false);

        base.ConfigureMocks(command, overrides);
    }

    public CreateRoleCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();

        commandHandler = new CreateRoleCommandHandler(roleRepositoryMock, discussionRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        ConfigureMocks(Command);

        // Act
        Result<Guid> result = await commandHandler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateRoleNamesInDiscussion_WhenDuplicateRoleNamesInDiscussionAsyncReturnsTrue()
    {
        // Arrange
        ConfigureMocks(Command, overrides: () =>
        {
            roleRepositoryMock
                .DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId))
                .Returns(true);
        });

        // Act
        Result<Guid> result = await commandHandler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.DuplicateRoleNamesInDiscussion);
    }

    [Fact]
    public async Task Handle_Should_ReturnPermissionNotAllowed_WhenAPermissionIsNotInListOfAllowedPermissions()
    {
        // Arrange
        string invalidPermission = "";
        
        CreateRoleCommand permissionNotAllowedCommand = new(
            Command.DiscussionId, Command.Name, [invalidPermission]);

        ConfigureMocks(permissionNotAllowedCommand);

        // Act
        Result<Guid> result = await commandHandler.Handle(permissionNotAllowedCommand, default);

        // Assert
        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionsAreInList()
    {
        // Arrange
        string duplicatePermission = Permission.KickUser.Value;

        CreateRoleCommand duplicatePermissionCommand = new(
            Command.DiscussionId, Command.Name, [duplicatePermission, duplicatePermission]);

        ConfigureMocks(duplicatePermissionCommand);

        // Act
        Result<Guid> result = await commandHandler.Handle(duplicatePermissionCommand, default);

        // Assert
        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryInsert()
    {
        // Arrange
        ConfigureMocks(Command);

        // Act
        Result<Guid> result = await commandHandler.Handle(Command, default);

        // Assert
        roleRepositoryMock
            .Received(1)
            .Insert(Arg.Is<Role>(r => r.Id == result.Value));
    }
}
