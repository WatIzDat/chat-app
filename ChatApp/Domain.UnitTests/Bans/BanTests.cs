using Domain.Bans;
using SharedKernel;

namespace Domain.UnitTests.Bans;

public class BanTests
{
    [Fact]
    public void UnbanUser_Should_ReturnSuccess()
    {
        // Arrange
        Ban ban = Ban.Create(
            Guid.Empty,
            Guid.Empty,
            DateTimeOffset.MinValue,
            BanDetails.CreatePermanentBan());

        // Act
        Result result = ban.UnbanUser();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void UnbanUser_Should_SetIsUnbannedToTrue()
    {
        // Arrange
        Ban ban = Ban.Create(
            Guid.Empty,
            Guid.Empty,
            DateTimeOffset.MinValue,
            BanDetails.CreatePermanentBan());

        // Act
        ban.UnbanUser();

        // Assert
        ban.IsUnbanned.Should().BeTrue();
    }
}
