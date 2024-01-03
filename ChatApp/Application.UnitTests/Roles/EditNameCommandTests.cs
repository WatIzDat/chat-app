using Application.Roles.EditName;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class EditNameCommandTests
{
    private static readonly Role Role = Role.Create(
        Guid.NewGuid(), "test", [Permission.BanUser, Permission.DeleteMessage]).Value;

    private readonly EditNameCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    public EditNameCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new EditNameCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        EditNameCommand command = new(role.Id, "hello");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        role.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        EditNameCommand command = new(role.Id, "hello");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateRoleNamesInDiscussion_WhenDuplicateRoleNamesInDiscussionAsyncReturnsTrue()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        EditNameCommand command = new(role.Id, "hello");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(command.Name, role.DiscussionId).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.DuplicateRoleNamesInDiscussion);
    }

    [Fact]
    public async Task Handle_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        EditNameCommand command = new(role.Id, "thisnameiswaytoolongaaaaaaaaaaaaaaaa");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(command.Name, role.DiscussionId).Returns(false);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.NameTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryUpdate()
    {
        Role role = Role.Create(
            Role.DiscussionId,
            Role.Name,
            Role.Permissions.ToList()).Value;

        EditNameCommand command = new(role.Id, "hello");

        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(command.Name, role.DiscussionId).Returns(false);

        Result result = await commandHandler.Handle(command, default);

        roleRepositoryMock
            .Received(1)
            .Update(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
