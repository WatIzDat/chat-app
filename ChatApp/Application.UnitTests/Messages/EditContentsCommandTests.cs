using Application.Messages.EditContents;
using Domain.Messages;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Messages;

public class EditContentsCommandTests
{
    private static readonly Message Message = Message.Create(
        Guid.NewGuid(), Guid.NewGuid(), "This is a test.", DateTimeOffset.UtcNow).Value;

    private readonly EditContentsCommandHandler commandHandler;
    private readonly IMessageRepository messageRepositoryMock;

    public EditContentsCommandTests()
    {
        messageRepositoryMock = Substitute.For<IMessageRepository>();

        commandHandler = new EditContentsCommandHandler(messageRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        EditContentsCommand command = new(message.Id, "hello");

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        message.Contents.Should().Be(command.Contents);
    }

    [Fact]
    public async Task Handle_Should_ReturnMessageNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        EditContentsCommand command = new(message.Id, "hello");

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(MessageErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        string contents = string.Empty;

        for (int i = 0; i < Message.ContentsMaxLength + 1; i++)
        {
            contents += "a";
        }

        EditContentsCommand command = new(message.Id, contents);

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallMessageRepositoryUpdate()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        EditContentsCommand command = new(message.Id, "hello");

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        Result result = await commandHandler.Handle(command, default);

        messageRepositoryMock
            .Received(1)
            .Update(Arg.Is<Message>(m => m.Id == command.MessageId));
    }
}
