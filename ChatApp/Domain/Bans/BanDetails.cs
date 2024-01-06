using SharedKernel;

namespace Domain.Bans;

public sealed record BanDetails
{
    private BanDetails(
        bool isBanPermanent,
        DateTimeOffset? dateOfUnbanUtc)
    {
        IsBanPermanent = isBanPermanent;
        DateOfUnbanUtc = dateOfUnbanUtc;
    }

    public bool IsBanPermanent { get; }

    public DateTimeOffset? DateOfUnbanUtc { get; }

    public static Result<BanDetails> CreateTemporaryBan(DateTimeOffset currentTime, DateTimeOffset dateOfUnbanUtc)
    {
        if (currentTime > dateOfUnbanUtc)
        {
            return Result.Failure<BanDetails>(BanErrors.CurrentTimeEarlierThanDateOfUnban);
        }

        BanDetails banDetails = new(isBanPermanent: false, dateOfUnbanUtc);

        return Result.Success(banDetails);
    }

    public static BanDetails CreatePermanentBan()
    {
        BanDetails banDetails = new(isBanPermanent: true, dateOfUnbanUtc: null);

        return banDetails;
    }
}
