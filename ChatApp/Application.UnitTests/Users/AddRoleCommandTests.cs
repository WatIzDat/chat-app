using Application.Users.AddRole;
using Domain.Roles;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class AddRoleCommandTests
{
    private static readonly Guid RoleId = new("ac6338e8-cb43-499a-b8c3-511ac099362e");

    private static readonly DiscussionsList Discussions =
        DiscussionsList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;

    private static readonly RolesList Roles =
        RolesList.Create([Guid.NewGuid()], Discussions).Value;

    private static readonly User User = User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            Discussions,
            Roles).Value;

    private readonly AddRoleCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;
    private readonly IRoleRepository roleRepositoryMock;

    public AddRoleCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new AddRoleCommandHandler(userRepositoryMock, roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        AddRoleCommand command = new(user.Id, RoleId);
        
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(true);
        roleRepositoryMock.RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        AddRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(true);
        roleRepositoryMock.RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenRoleExistsAsyncReturnsFalse()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        AddRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(false);
        roleRepositoryMock.RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotInDiscussionsList_WhenRoleInDiscussionsListAsyncReturnsFalse()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        AddRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(true);
        roleRepositoryMock.RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions)).Returns(false);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.RoleNotInDiscussionsList);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateRoles_WhenDuplicateGuidIsAdded()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        AddRoleCommand command = new(user.Id, user.Roles.Value[0]);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(true);
        roleRepositoryMock.RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(RolesListErrors.DuplicateRoles);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        AddRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(true);
        roleRepositoryMock.RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
