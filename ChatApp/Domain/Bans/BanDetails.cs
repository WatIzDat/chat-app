using SharedKernel;

namespace Domain.Bans;

public sealed record BanDetails
{
    private BanDetails(
        bool isBanPermanent,
        DateTimeOffset? dateWillBeUnbannedUtc)
    {
        IsBanPermanent = isBanPermanent;
        DateWillBeUnbannedUtc = dateWillBeUnbannedUtc;
    }

    public bool IsBanPermanent { get; }

    public DateTimeOffset? DateWillBeUnbannedUtc { get; }

    public static Result<BanDetails> CreateTemporaryBan(DateTimeOffset currentTime, DateTimeOffset dateWillBeUnbanned)
    {
        if (currentTime > dateWillBeUnbanned)
        {
            return Result.Failure<BanDetails>(BanErrors.CurrentTimePastDateWillBeUnbanned);
        }

        BanDetails banDetails = new(isBanPermanent: false, dateWillBeUnbanned);

        return Result.Success(banDetails);
    }

    public static BanDetails CreatePermanentBan()
    {
        BanDetails banDetails = new(isBanPermanent: true, dateWillBeUnbannedUtc: null);

        return banDetails;
    }
}
