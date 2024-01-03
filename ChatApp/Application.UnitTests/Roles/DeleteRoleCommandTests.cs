using Application.Roles.DeleteRole;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class DeleteRoleCommandTests
{
    private static readonly Role Role = Role.Create(
        Guid.NewGuid(), "test", [Permission.BanUser, Permission.DeleteMessage]).Value;

    private readonly DeleteRoleCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    public DeleteRoleCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new DeleteRoleCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        DeleteRoleCommand command = new(role.Id);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        DeleteRoleCommand command = new(role.Id);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryDelete()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        DeleteRoleCommand command = new(role.Id);

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        roleRepositoryMock
            .Received(1)
            .Delete(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
