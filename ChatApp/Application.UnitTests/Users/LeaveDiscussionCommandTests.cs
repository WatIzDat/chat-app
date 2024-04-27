using Application.Users.LeaveDiscussion;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class LeaveDiscussionCommandTests
{
    private static readonly Guid DiscussionId = new("ac6338e8-cb43-499a-b8c3-511ac099362e");

    private static readonly DiscussionsList Discussions =
        DiscussionsList.Create([DiscussionId]).Value;

    private static readonly RolesList Roles =
        RolesList.Create([Guid.NewGuid()]).Value;

    private static readonly User User = User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            Discussions,
            Roles,
            "test").Value;

    private readonly LeaveDiscussionCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    public LeaveDiscussionCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new LeaveDiscussionCommandHandler(userRepositoryMock);
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

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        user.Discussions.Value.Should().NotContain(command.DiscussionId);
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

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenGuidIsNotInDiscussionsList()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles,
            User.ClerkId).Value;

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        LeaveDiscussionCommand discussionNotFoundCommand = new(
            command.UserId,
            Guid.NewGuid());

        Result result = await commandHandler.Handle(discussionNotFoundCommand, default);

        result.Error.Should().Be(UserErrors.DiscussionNotFound);
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

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
