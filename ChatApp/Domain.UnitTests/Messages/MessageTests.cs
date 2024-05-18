using Domain.Messages;
using SharedKernel;

namespace Domain.UnitTests.Messages;

public class MessageTests
{
    private static Message CreateDefaultMessage()
    {
        return Message.Create(
            Guid.Empty,
            Guid.Empty,
            "test",
            DateTimeOffset.MinValue).Value;
    }

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        Message referenceMessage = CreateDefaultMessage();

        // Act
        Result<Message> result = Message.Create(
            referenceMessage.UserId,
            referenceMessage.DiscussionId,
            referenceMessage.Contents,
            referenceMessage.DateSentUtc);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        // Arrange
        Message referenceMessage = CreateDefaultMessage();

        string contentsLongerThanMaxLength = string.Empty.PadLeft(Message.ContentsMaxLength + 1);

        // Act
        Result<Message> result = Message.Create(
            referenceMessage.UserId,
            referenceMessage.DiscussionId,
            contentsLongerThanMaxLength,
            referenceMessage.DateSentUtc);

        // Assert
        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }

    [Fact]
    public void EditContents_Should_ReturnSuccess()
    {
        // Arrange
        Message defaultMessage = CreateDefaultMessage();

        string validContents = "a";

        // Act
        Result result = defaultMessage.EditContents(validContents);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void EditContents_Should_ChangeContents()
    {
        // Arrange
        Message defaultMessage = CreateDefaultMessage();

        string validContents = "a";

        // Act
        defaultMessage.EditContents(validContents);

        // Assert
        defaultMessage.Contents.Should().Be(validContents);
    }

    [Fact]
    public void EditContents_Should_SetIsEditedToTrue()
    {
        // Arrange
        Message defaultMessage = CreateDefaultMessage();

        string validContents = "a";

        // Act
        defaultMessage.EditContents(validContents);

        // Assert
        defaultMessage.IsEdited.Should().BeTrue();
    }

    [Fact]
    public void EditContents_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        // Arrange
        Message defaultMessage = CreateDefaultMessage();

        string contentsLongerThanMaxLength = string.Empty.PadLeft(Message.ContentsMaxLength + 1);

        // Act
        Result result = defaultMessage.EditContents(contentsLongerThanMaxLength);

        // Assert
        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }
}
