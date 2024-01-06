using Application.Discussions.CreateDiscussion;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.UnitTests.Discussions;

public class CreateDiscussionCommandTests
{
    private static readonly CreateDiscussionCommand Command = new(
        Guid.NewGuid(), "Test");

    private readonly CreateDiscussionCommandHandler commandHandler;

    private readonly IDiscussionRepository discussionRepositoryMock;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProviderMock;

    public CreateDiscussionCommandTests()
    {
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();
        userRepositoryMock = Substitute.For<IUserRepository>();
        dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();

        commandHandler = new CreateDiscussionCommandHandler(discussionRepositoryMock, userRepositoryMock, dateTimeOffsetProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenUserExistsAsyncReturnsFalse()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(false);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);

        CreateDiscussionCommand nameTooLongCommand = new(
            Command.UserId, "thisnameiswaytoolongaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        Result<Guid> result = await commandHandler.Handle(nameTooLongCommand, default);

        result.Error.Should().Be(DiscussionErrors.NameTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallDiscussionRepositoryInsert()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        discussionRepositoryMock
            .Received(1)
            .Insert(Arg.Is<Discussion>(d => d.Id == result.Value));
    }
}
