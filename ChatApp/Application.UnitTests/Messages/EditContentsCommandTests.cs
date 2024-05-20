using Application.Messages.EditContents;
using Domain.Messages;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Messages;

public class EditContentsCommandTests : BaseMessageTest<EditContentsCommand>
{
    private const string ValidContents = "test";

    private readonly EditContentsCommandHandler commandHandler;
    private readonly IMessageRepository messageRepositoryMock;

    protected override void ConfigureMocks(Message message, EditContentsCommand command, Action? overrides = null)
    {
        messageRepositoryMock.GetByIdAsync(Arg.Is(command.MessageId)).Returns(message);

        base.ConfigureMocks(message, command, overrides);
    }

    public EditContentsCommandTests()
    {
        messageRepositoryMock = Substitute.For<IMessageRepository>();

        commandHandler = new EditContentsCommandHandler(messageRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        EditContentsCommand command = new(message.Id, ValidContents);

        ConfigureMocks(message, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_EditContents()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        EditContentsCommand command = new(message.Id, ValidContents);

        ConfigureMocks(message, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        message.Contents.Should().Be(command.Contents);
    }

    [Fact]
    public async Task Handle_Should_ReturnMessageNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        EditContentsCommand command = new(message.Id, ValidContents);

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
    public async Task Handle_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        string contentsLongerThanMaxLength = string.Empty.PadLeft(Message.ContentsMaxLength + 1);

        EditContentsCommand command = new(message.Id, contentsLongerThanMaxLength);

        ConfigureMocks(message, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallMessageRepositoryUpdate()
    {
        // Arrange
        Message message = CreateDefaultMessage();

        EditContentsCommand command = new(message.Id, ValidContents);

        ConfigureMocks(message, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        messageRepositoryMock
            .Received(1)
            .Update(Arg.Is<Message>(m => m.Id == command.MessageId));
    }
}
