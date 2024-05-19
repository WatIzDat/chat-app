using Application.Users.LeaveDiscussion;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class LeaveDiscussionCommandTests : BaseUserTest<LeaveDiscussionCommand>
{
    protected override DiscussionsList CreateDefaultDiscussionsList()
    {
        return DiscussionsList.Create([DiscussionId]).Value;
    }

    private static readonly Guid DiscussionId = Guid.Empty;

    private readonly LeaveDiscussionCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    protected override void ConfigureMocks(User user, LeaveDiscussionCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        base.ConfigureMocks(user, command, overrides);
    }

    public LeaveDiscussionCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new LeaveDiscussionCommandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_RemoveDiscussion()
    {
        // Arrange
        User user = CreateDefaultUser();

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        user.Discussions.Value.Should().NotContain(command.DiscussionId);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

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
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenGuidIsNotInDiscussionsList()
    {
        // Arrange
        User user = CreateDefaultUser();

        LeaveDiscussionCommand command = new(user.Id, Guid.NewGuid());

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(UserErrors.DiscussionNotFound);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        LeaveDiscussionCommand command = new(user.Id, DiscussionId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
