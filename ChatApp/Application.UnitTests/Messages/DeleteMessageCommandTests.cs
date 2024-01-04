using Application.Messages.DeleteMessage;
using Domain.Messages;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Messages;

public class DeleteMessageCommandTests
{
    private static readonly Message Message = Message.Create(
        Guid.NewGuid(), Guid.NewGuid(), "This is a test.", DateTimeOffset.UtcNow).Value;

    private readonly DeleteMessageCommandHandler commandHandler;
    private readonly IMessageRepository messageRepositoryMock;

    public DeleteMessageCommandTests()
    {
        messageRepositoryMock = Substitute.For<IMessageRepository>();

        commandHandler = new DeleteMessageCommandHandler(messageRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        DeleteMessageCommand command = new(message.Id);

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnMessageNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        DeleteMessageCommand command = new(message.Id);

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(MessageErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_CallMessageRepositoryDelete()
    {
        Message message = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc).Value;

        DeleteMessageCommand command = new(message.Id);

        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        Result result = await commandHandler.Handle(command, default);

        messageRepositoryMock
            .Received(1)
            .Delete(Arg.Is<Message>(m => m.Id == command.MessageId));
    }
}
