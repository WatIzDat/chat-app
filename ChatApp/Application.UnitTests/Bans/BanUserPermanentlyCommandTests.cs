using Application.Bans.BanUserPermanently;
using Domain.Bans;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.UnitTests.Bans;

public class BanUserPermanentlyCommandTests
{
    private static readonly BanUserPermanentlyCommand Command = new(
        Guid.NewGuid(), Guid.NewGuid());

    private readonly BanUserPermanentlyCommandHandler commandHandler;

    private readonly IBanRepository banRepositoryMock;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDiscussionRepository discussionRepositoryMock;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProviderMock;


    public BanUserPermanentlyCommandTests()
    {
        banRepositoryMock = Substitute.For<IBanRepository>();
        userRepositoryMock = Substitute.For<IUserRepository>();
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();
        dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();

        commandHandler = new BanUserPermanentlyCommandHandler(
            banRepositoryMock,
            userRepositoryMock,
            discussionRepositoryMock,
            dateTimeOffsetProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenUserExistsAsyncReturnsFalse()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(false);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);

        Result result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenDiscussionExistsAsyncReturnsFalse()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(false);

        Result result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(DiscussionErrors.NotFound);
    }
}
