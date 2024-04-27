using Application.Users.RemoveRole;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class RemoveRoleCommandTests
{
    private static readonly Guid RoleId = new("ac6338e8-cb43-499a-b8c3-511ac099362e");

    private static readonly DiscussionsList Discussions =
        DiscussionsList.Create([Guid.NewGuid()]).Value;

    private static readonly RolesList Roles =
        RolesList.Create([RoleId]).Value;

    private static readonly User User = User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            Discussions,
            Roles,
            "test").Value;

    private readonly RemoveRoleCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    public RemoveRoleCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new RemoveRoleCommandHandler(userRepositoryMock);
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
            User.Roles,
            User.ClerkId).Value;

        RemoveRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        user.Roles.Value.Should().NotContain(command.RoleId);
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
            User.Roles,
            User.ClerkId).Value;

        RemoveRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGuidIsNotInRolesList()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles,
            User.ClerkId).Value;

        RemoveRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        RemoveRoleCommand roleNotFoundCommand = new(
            command.UserId,
            Guid.NewGuid());

        Result result = await commandHandler.Handle(roleNotFoundCommand, default);

        result.Error.Should().Be(UserErrors.RoleNotFound);
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
            User.Roles,
            User.ClerkId).Value;

        RemoveRoleCommand command = new(user.Id, RoleId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
