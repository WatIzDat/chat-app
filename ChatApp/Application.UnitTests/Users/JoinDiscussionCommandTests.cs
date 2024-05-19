using Application.Users.JoinDiscussion;
using Domain.Discussions;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class JoinDiscussionCommandTests : BaseUserTest<JoinDiscussionCommand>
{
    private static readonly Guid DiscussionId = Guid.Empty;

    private readonly JoinDiscussionCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDiscussionRepository discussionRepositoryMock;

    protected override void ConfigureMocks(User user, JoinDiscussionCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(true);

        base.ConfigureMocks(user, command, overrides);
    }

    public JoinDiscussionCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();

        commandHandler = new JoinDiscussionCommandHandler(userRepositoryMock, discussionRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        JoinDiscussionCommand command = new(user.Id, DiscussionId);
        
        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command, overrides: () =>
        {
            userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenDiscussionExistsAsyncReturnsFalse()
    {
        // Arrange
        User user = CreateDefaultUser();

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command, overrides: () =>
        {
            discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(false);
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(DiscussionErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateDiscussions_WhenDuplicateGuidIsAdded()
    {
        // Arrange
        User user = CreateDefaultUser();

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command);

        await commandHandler.Handle(command, default);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(DiscussionsListErrors.DuplicateDiscussions);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        JoinDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
