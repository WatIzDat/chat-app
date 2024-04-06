using Domain.Bans;
using SharedKernel;

namespace Domain.UnitTests.Bans;

public class BanDetailsTests
{
    [Fact]
    public void CreateTemporaryBan_Should_ReturnBanDetails_WhereIsBanPermanentIsFalse_And_DateOfUnbanIsSet()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        BanDetails banDetails = BanDetails.CreateTemporaryBan(utcNow, utcNow.AddDays(1)).Value;

        banDetails.IsBanPermanent.Should().BeFalse();
        banDetails.DateWillBeUnbannedUtc.Should().Be(utcNow.AddDays(1));
    }

    [Fact]
    public void CreatePermanentBan_Should_ReturnBanDetails_WhereIsBanPermanentIsTrue_And_DateOfUnbanIsNull()
    {
        BanDetails banDetails = BanDetails.CreatePermanentBan();

        banDetails.IsBanPermanent.Should().BeTrue();
        banDetails.DateWillBeUnbannedUtc.Should().BeNull();
    }

    [Fact]
    public void CreateTemporaryBan_Should_ReturnCurrentTimePastDateWillBeUnbanned_WhenCurrentTimeIsPastDateWillBeUnbanned()
    {
        Result result = BanDetails.CreateTemporaryBan(
            DateTimeOffset.UtcNow.AddDays(1),
            DateTimeOffset.UtcNow);

        result.Error.Should().Be(BanErrors.CurrentTimePastDateWillBeUnbanned);
    }
}
