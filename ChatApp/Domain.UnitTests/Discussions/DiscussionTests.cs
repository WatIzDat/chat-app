using Domain.Discussions;
using SharedKernel;

namespace Domain.UnitTests.Discussions;

public class DiscussionTests
{
    private static readonly Discussion Discussion = Discussion.Create(
        Guid.NewGuid(),
        "test",
        DateTimeOffset.UtcNow).Value;

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        Result<Discussion> result = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        Result<Discussion> result = Discussion.Create(
            Discussion.UserCreatedBy,
            "this name is too longaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            Discussion.DateCreatedUtc);

        result.Error.Should().Be(DiscussionErrors.NameTooLong);
    }

    [Fact]
    public void EditName_Should_ReturnSuccess_And_ChangeName()
    {
        Result result = Discussion.EditName("hello");

        result.IsSuccess.Should().BeTrue();

        Discussion.Name.Should().Be("hello");
    }

    [Fact]
    public void EditName_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        Result result = Discussion.EditName("this name is too longaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        result.Error.Should().Be(DiscussionErrors.NameTooLong);
    }
}
