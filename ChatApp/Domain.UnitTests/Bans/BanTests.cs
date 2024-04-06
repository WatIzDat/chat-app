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

        Result result = ban.UnbanUser();

        result.IsSuccess.Should().BeTrue();

        ban.IsUnbanned.Should().BeTrue();
    }
}
