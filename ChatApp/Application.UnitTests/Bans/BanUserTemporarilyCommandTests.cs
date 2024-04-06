using Application.Bans.BanUserTemporarily;
using Domain.Bans;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.UnitTests.Bans;

public class BanUserTemporarilyCommandTests
{
    private static readonly BanUserTemporarilyCommand Command = new(
        Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1));

    private readonly BanUserTemporarilyCommandHandler commandHandler;

    private readonly IBanRepository banRepositoryMock;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDiscussionRepository discussionRepositoryMock;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProviderMock;

    public BanUserTemporarilyCommandTests()
    {
        banRepositoryMock = Substitute.For<IBanRepository>();
        userRepositoryMock = Substitute.For<IUserRepository>();
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();
        dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();

        commandHandler = new BanUserTemporarilyCommandHandler(
            banRepositoryMock,
            userRepositoryMock,
            discussionRepositoryMock,
            dateTimeOffsetProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        banRepositoryMock.BanExistsByUserAndDiscussionIdAsync(
            Arg.Is(Command.UserId), Arg.Is(Command.DiscussionId)).Returns(false);

        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserAlreadyBannedFromDiscussion_WhenBanExistsByUserAndDiscussionIdAsyncReturnsTrue()
    {
        banRepositoryMock.BanExistsByUserAndDiscussionIdAsync(
            Arg.Is(Command.UserId), Arg.Is(Command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(BanErrors.UserAlreadyBannedFromDiscussion);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenUserExistsAsyncReturnsFalse()
    {
        banRepositoryMock.BanExistsByUserAndDiscussionIdAsync(
            Arg.Is(Command.UserId), Arg.Is(Command.DiscussionId)).Returns(false);

        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(false);

        Result result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenDiscussionExistsAsyncReturnsFalse()
    {
        banRepositoryMock.BanExistsByUserAndDiscussionIdAsync(
            Arg.Is(Command.UserId), Arg.Is(Command.DiscussionId)).Returns(false);

        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(false);

        Result result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(DiscussionErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnCurrentTimePastDateWillBeUnbanned_WhenCurrentTimeIsPastDateWillBeUnbanned()
    {
        BanUserTemporarilyCommand command = new(
            Command.UserId,
            Command.DiscussionId,
            DateTimeOffset.UtcNow.AddDays(-1));

        banRepositoryMock.BanExistsByUserAndDiscussionIdAsync(
            Arg.Is(command.UserId), Arg.Is(command.DiscussionId)).Returns(false);

        userRepositoryMock.UserExistsAsync(Arg.Is(command.UserId)).Returns(true);

        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(command.DiscussionId)).Returns(true);

        dateTimeOffsetProviderMock.UtcNow.Returns(DateTimeOffset.UtcNow);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(BanErrors.CurrentTimePastDateWillBeUnbanned);
    }
}
