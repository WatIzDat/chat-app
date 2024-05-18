using Domain.Bans;
using SharedKernel;

namespace Domain.UnitTests.Bans;

public class BanDetailsTests
{
    [Fact]
    public void CreateTemporaryBan_Should_ReturnBanDetails_Where_IsBanPermanentIsFalse()
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.MinValue;
        DateTimeOffset dateWillBeUnbanned = currentTime.AddDays(1);

        // Act
        BanDetails banDetails = BanDetails.CreateTemporaryBan(currentTime, dateWillBeUnbanned).Value;

        // Assert
        banDetails.IsBanPermanent.Should().BeFalse();
    }

    [Fact]
    public void CreateTemporaryBan_Should_ReturnBanDetails_Where_DateOfUnbanIsSet()
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.MinValue;
        DateTimeOffset dateWillBeUnbanned = currentTime.AddDays(1);

        // Act
        BanDetails banDetails = BanDetails.CreateTemporaryBan(currentTime, dateWillBeUnbanned).Value;

        // Assert
        banDetails.DateWillBeUnbannedUtc.Should().Be(dateWillBeUnbanned);
    }

    [Fact]
    public void CreatePermanentBan_Should_ReturnBanDetails_Where_IsBanPermanentIsTrue()
    {
        // Act
        BanDetails banDetails = BanDetails.CreatePermanentBan();

        // Assert
        banDetails.IsBanPermanent.Should().BeTrue();
    }

    [Fact]
    public void CreatePermanentBan_Should_ReturnBanDetails_Where_DateOfUnbanIsNull()
    {
        // Act
        BanDetails banDetails = BanDetails.CreatePermanentBan();

        // Assert
        banDetails.DateWillBeUnbannedUtc.Should().BeNull();
    }

    [Fact]
    public void CreateTemporaryBan_Should_ReturnCurrentTimePastDateWillBeUnbanned_WhenCurrentTimeIsPastDateWillBeUnbanned()
    {
        // Arrange
        DateTimeOffset dateWillBeUnbanned = DateTimeOffset.MinValue;
        DateTimeOffset currentTimePastDateWillBeUnbanned = dateWillBeUnbanned.AddDays(1);

        // Act
        Result result = BanDetails.CreateTemporaryBan(
            currentTimePastDateWillBeUnbanned,
            dateWillBeUnbanned);

        // Assert
        result.Error.Should().Be(BanErrors.CurrentTimePastDateWillBeUnbanned);
    }
}
