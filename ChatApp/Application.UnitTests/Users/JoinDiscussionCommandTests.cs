using Application.Users.JoinDiscussion;
using Domain.Discussions;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class JoinDiscussionCommandTests
{
    private static readonly Guid DiscussionId = new("ac6338e8-cb43-499a-b8c3-511ac099362e");

    private static readonly DiscussionsList Discussions =
        DiscussionsList.Create([Guid.NewGuid()]).Value;

    private static readonly RolesList Roles =
        RolesList.Create([Guid.NewGuid()], Discussions).Value;

    private static readonly User User = User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            Discussions,
            Roles).Value;

    private readonly JoinDiscussionCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDiscussionRepository discussionRepositoryMock;

    public JoinDiscussionCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();

        commandHandler = new JoinDiscussionCommandHandler(userRepositoryMock, discussionRepositoryMock);
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

        JoinDiscussionCommand command = new(user.Id, DiscussionId);
        
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(true);

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

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenDiscussionExistsAsyncReturnsFalse()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(false);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(DiscussionErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateDiscussions_WhenDuplicateGuidIsAdded()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        JoinDiscussionCommand command = new(user.Id, user.Discussions.Value[0]);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(DiscussionsListErrors.DuplicateDiscussions);
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

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
