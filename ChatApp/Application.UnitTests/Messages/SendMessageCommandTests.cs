using Application.Messages.SendMessage;
using Domain.Discussions;
using Domain.Messages;
using Domain.Users;
using SharedKernel;

namespace Application.UnitTests.Messages;

public class SendMessageCommandTests
{
    private static readonly SendMessageCommand Command = new(
        Guid.NewGuid(), Guid.NewGuid(), "This is a test.");

    private readonly SendMessageCommandHandler commandHandler;

    private readonly IMessageRepository messageRepositoryMock;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDiscussionRepository discussionRepositoryMock;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProviderMock;

    public SendMessageCommandTests()
    {
        messageRepositoryMock = Substitute.For<IMessageRepository>();
        userRepositoryMock = Substitute.For<IUserRepository>();
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();
        dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();

        commandHandler = new SendMessageCommandHandler(
            messageRepositoryMock,
            userRepositoryMock,
            discussionRepositoryMock,
            dateTimeOffsetProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);

        string contents = string.Empty;

        for (int i = 0; i < Message.ContentsMaxLength + 1; i++)
        {
            contents += "a";
        }

        SendMessageCommand contentsTooLongCommand = new(
            Command.UserId,
            Command.DiscussionId,
            contents);

        Result<Guid> result = await commandHandler.Handle(contentsTooLongCommand, default);

        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallMessageRepositoryInsert()
    {
        userRepositoryMock.UserExistsAsync(Arg.Is(Command.UserId)).Returns(true);
        discussionRepositoryMock.DiscussionExistsAsync(Arg.Is(Command.DiscussionId)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);
        
        messageRepositoryMock
            .Received(1)
            .Insert(Arg.Is<Message>(m => m.Id == result.Value));
    }
}
