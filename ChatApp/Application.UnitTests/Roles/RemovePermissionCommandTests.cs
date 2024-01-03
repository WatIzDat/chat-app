using Application.Roles.RemovePermission;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class RemovePermissionCommandTests
{
    private static readonly Role Role = Role.Create(
        Guid.NewGuid(), "test", [Permission.BanUser, Permission.DeleteMessage]).Value;

    private readonly RemovePermissionCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    public RemovePermissionCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new RemovePermissionCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        RemovePermissionCommand command = new(role.Id, Permission.BanUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        role.Permissions.Should().NotContain(Permission.BanUser);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        RemovePermissionCommand command = new(role.Id, Permission.BanUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnPermissionNotAllowed_WhenPermissionIsNotInListOfAllowedPermissions()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        RemovePermissionCommand command = new(role.Id, "this permission is not allowed");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnPermissionNotFound_WhenPermissionIsNotInPermissionsList()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        RemovePermissionCommand command = new(role.Id, Permission.KickUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.PermissionNotFound);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryUpdate()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        RemovePermissionCommand command = new(role.Id, Permission.BanUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        roleRepositoryMock
            .Received(1)
            .Update(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
