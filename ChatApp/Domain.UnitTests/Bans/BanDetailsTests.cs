using Domain.Bans;

namespace Domain.UnitTests.Bans;

public class BanDetailsTests
{
    [Fact]
    public void CreateTemporaryBan_Should_ReturnBanDetails_WhereIsBanPermanentIsFalse_And_DateOfUnbanIsSet()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        BanDetails banDetails = BanDetails.CreateTemporaryBan(utcNow);

        banDetails.IsBanPermanent.Should().BeFalse();
        banDetails.DateOfUnbanUtc.Should().Be(utcNow);
    }

    [Fact]
    public void CreatePermanentBan_Should_ReturnBanDetails_WhereIsBanPermanentIsTrue_And_DateOfUnbanIsNull()
    {
        BanDetails banDetails = BanDetails.CreatePermanentBan();

        banDetails.IsBanPermanent.Should().BeTrue();
        banDetails.DateOfUnbanUtc.Should().BeNull();
    }
}
