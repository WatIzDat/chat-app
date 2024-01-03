using Application.Roles.AddPermission;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class AddPermissionCommandTests
{
    private static readonly Role Role = Role.Create(
        Guid.NewGuid(), "test", [Permission.BanUser, Permission.DeleteMessage]).Value;

    private readonly AddPermissionCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    public AddPermissionCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new AddPermissionCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        role.Permissions.Should().Contain(Permission.KickUser);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

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

        AddPermissionCommand command = new(role.Id, "this permission is not allowed");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(PermissionErrors.NotAllowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicatePermissions_WhenDuplicatePermissionIsAdded()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        AddPermissionCommand command = new(role.Id, Permission.BanUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.DuplicatePermissions);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryUpdate()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        AddPermissionCommand command = new(role.Id, Permission.KickUser.Value);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        roleRepositoryMock
            .Received(1)
            .Update(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
