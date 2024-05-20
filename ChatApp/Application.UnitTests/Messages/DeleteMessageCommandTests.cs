using Application.Messages.DeleteMessage;
using Domain.Messages;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Messages;

public class DeleteMessageCommandTests : BaseMessageTest<DeleteMessageCommand>
{
    private readonly DeleteMessageCommandHandler commandHandler;
    private readonly IMessageRepository messageRepositoryMock;

    protected override void ConfigureMocks(Message message, DeleteMessageCommand command, Action? overrides = null)
    {
        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        base.ConfigureMocks(message, command, overrides);
    }

    public DeleteMessageCommandTests()
    {
        messageRepositoryMock = Substitute.For<IMessageRepository>();

        commandHandler = new DeleteMessageCommandHandler(messageRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        DeleteMessageCommand command = new(message.Id);

        ConfigureMocks(message, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnMessageNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        DeleteMessageCommand command = new(message.Id);

        ConfigureMocks(message, command, overrides: () =>
        {
            messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).ReturnsNull();
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(MessageErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_CallMessageRepositoryDelete()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        DeleteMessageCommand command = new(message.Id);

        ConfigureMocks(message, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        messageRepositoryMock
            .Received(1)
            .Delete(Arg.Is<Message>(m => m.Id == command.MessageId));
    }
}
