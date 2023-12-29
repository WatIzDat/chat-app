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

    public static BanDetails CreateTemporaryBan(DateTimeOffset dateOfUnbanUtc)
    {
        BanDetails banDetails = new(isBanPermanent: false, dateOfUnbanUtc);

        return banDetails;
    }

    public static BanDetails CreatePermanentBan()
    {
        BanDetails banDetails = new(isBanPermanent: true, dateOfUnbanUtc: null);

        return banDetails;
    }
}
