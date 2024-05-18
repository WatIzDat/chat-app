using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class DiscussionsListTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        Guid testGuid = Guid.Empty; 

        // Act
        Result<DiscussionsList> result = DiscussionsList.Create([testGuid]);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnTooManyDiscussions_WhenListIsLongerThanMaxLength()
    {
        // Arrange
        List<Guid> discussions = [];

        for (int i = 0; i < DiscussionsList.MaxLength + 1; i++)
        {
            discussions.Add(Guid.NewGuid());
        }

        // Act
        Result<DiscussionsList> result = DiscussionsList.Create(discussions);

        // Assert
        result.Error.Should().Be(DiscussionsListErrors.TooManyDiscussions);
    }

    [Fact]
    public void Create_Should_ReturnDuplicateDiscussions_WhenDuplicateGuidsAreInList()
    {
        // Arrange
        Guid testGuid = Guid.Empty;

        // Act
        Result<DiscussionsList> result = DiscussionsList.Create([testGuid, testGuid]);

        // Assert
        result.Error.Should().Be(DiscussionsListErrors.DuplicateDiscussions);
    }
}
