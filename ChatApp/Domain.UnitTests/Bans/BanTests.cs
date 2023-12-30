using Domain.Bans;
using SharedKernel;

namespace Domain.UnitTests.Bans;

public class BanTests
{
    [Fact]
    public void UnbanUser_Should_ReturnSuccess_And_SetIsUnbannedToTrue()
    {
        Ban ban = Ban.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            BanDetails.CreatePermanentBan());

        Result result = ban.UnbanUser(DateTimeOffset.UtcNow);

        result.IsSuccess.Should().BeTrue();

        ban.IsUnbanned.Should().BeTrue();
    }

    [Fact]
    public void UnbanUser_Should_ReturnCurrentTimeEarlierThanDateOfUnban_WhenUtcNowIsEarlierThanDateOfUnban()
    {
        Ban ban = Ban.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            BanDetails.CreateTemporaryBan(DateTimeOffset.UtcNow.AddDays(1)));

        Result result = ban.UnbanUser(DateTimeOffset.UtcNow);

        result.Error.Should().Be(BanErrors.CurrentTimeEarlierThanDateOfUnban);
    }
}
