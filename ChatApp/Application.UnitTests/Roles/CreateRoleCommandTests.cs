using Application.Roles.CreateRole;
using Domain.Roles;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class CreateRoleCommandTests
{
    private static readonly CreateRoleCommand Command = new(
        Guid.NewGuid(), "test", [Permission.BanUser.Value, Permission.DeleteMessage.Value]);

    private readonly CreateRoleCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    public CreateRoleCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new CreateRoleCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId)).Returns(false);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateRoleNamesInDiscussion_WhenDuplicateRoleNamesInDiscussionAsyncReturnsTrue()
    {
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(RoleErrors.DuplicateRoleNamesInDiscussion);
    }

    [Fact]
    public async Task Handle_Should_ReturnPermissionNotAllowed_WhenAPermissionIsNotInListOfAllowedPermissions()
    {
        CreateRoleCommand permissionNotAllowedCommand = new(
            Command.DiscussionId, Command.Name, [Permission.BanUser.Value, "this permission is not allowed"]);

        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId)).Returns(false);

        Result<Guid> result = await commandHandler.Handle(permissionNotAllowedCommand, default);

        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionsAreInList()
    {
        CreateRoleCommand duplicatePermissionCommand = new(
            Command.DiscussionId, Command.Name, [Permission.BanUser.Value, Permission.BanUser.Value]);

        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId)).Returns(false);

        Result<Guid> result = await commandHandler.Handle(duplicatePermissionCommand, default);

        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryInsert()
    {
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(Arg.Is(Command.Name), Arg.Is(Command.DiscussionId)).Returns(false);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        roleRepositoryMock
            .Received(1)
            .Insert(Arg.Is<Role>(r => r.Id == result.Value));
    }
}
