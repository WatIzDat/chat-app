using Domain.Messages;
using SharedKernel;

namespace Domain.UnitTests.Messages;

public class MessageTests
{
    private static readonly Message Message = Message.Create(
        Guid.NewGuid(),
        Guid.NewGuid(),
        "This is a message.",
        DateTimeOffset.UtcNow).Value;

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        Result<Message> result = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            Message.Contents,
            Message.DateSentUtc);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        string contents = string.Empty;

        for (int i = 0; i < Message.ContentsMaxLength + 1; i++)
        {
            contents += "a";
        }

        Result<Message> result = Message.Create(
            Message.UserId,
            Message.DiscussionId,
            contents,
            Message.DateSentUtc);

        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }

    [Fact]
    public void EditContents_Should_ReturnSuccess_And_ChangeContents_And_SetIsEditedToTrue()
    {
        Result result = Message.EditContents("hello");

        result.IsSuccess.Should().BeTrue();

        Message.Contents.Should().Be("hello");
        Message.IsEdited.Should().Be(true);
    }

    [Fact]
    public void EditContents_Should_ReturnContentsTooLong_WhenContentsIsLongerThanMaxLength()
    {
        string contents = string.Empty;

        for (int i = 0; i < Message.ContentsMaxLength + 1; i++)
        {
            contents += "a";
        }

        Result result = Message.EditContents(contents);

        result.Error.Should().Be(MessageErrors.ContentsTooLong);
    }
}
