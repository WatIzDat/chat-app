using Domain.Discussions;
using SharedKernel;

namespace Domain.UnitTests.Discussions;

public class DiscussionTests
{
    private static Discussion CreateDefaultDiscussion()
    {
        return Discussion.Create(
            Guid.Empty,
            "test",
            DateTimeOffset.MinValue).Value;
    }

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        Discussion referenceDiscussion = CreateDefaultDiscussion();

        // Act
        Result<Discussion> result = Discussion.Create(
            referenceDiscussion.UserCreatedBy,
            referenceDiscussion.Name,
            referenceDiscussion.DateCreatedUtc);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        // Arrange
        Discussion referenceDiscussion = CreateDefaultDiscussion();

        string nameLongerThanMaxLength = string.Empty.PadLeft(Discussion.NameMaxLength + 1);

        // Act
        Result<Discussion> result = Discussion.Create(
            referenceDiscussion.UserCreatedBy,
            nameLongerThanMaxLength,
            referenceDiscussion.DateCreatedUtc);

        // Assert
        result.Error.Should().Be(DiscussionErrors.NameTooLong);
    }

    [Fact]
    public void EditName_Should_ReturnSuccess()
    {
        // Arrange
        Discussion defaultDiscussion = CreateDefaultDiscussion();

        string validName = "a";

        // Act
        Result result = defaultDiscussion.EditName(validName);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void EditName_Should_ChangeName()
    {
        // Arrange
        Discussion defaultDiscussion = CreateDefaultDiscussion();

        string validName = "a";

        // Act
        defaultDiscussion.EditName(validName);

        // Assert
        defaultDiscussion.Name.Should().Be(validName);
    }

    [Fact]
    public void EditName_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        // Arrange
        Discussion defaultDiscussion = CreateDefaultDiscussion();

        string nameLongerThanMaxLength = string.Empty.PadLeft(Discussion.NameMaxLength + 1);

        // Act
        Result result = defaultDiscussion.EditName(nameLongerThanMaxLength);

        // Assert
        result.Error.Should().Be(DiscussionErrors.NameTooLong);
    }

    [Fact]
    public void Delete_Should_SetIsDeletedToTrue()
    {
        // Arrange
        Discussion defaultDiscussion = CreateDefaultDiscussion();

        // Act
        defaultDiscussion.Delete();

        // Assert
        defaultDiscussion.IsDeleted.Should().BeTrue();
    }
}
