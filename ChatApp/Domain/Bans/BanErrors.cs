using SharedKernel;

namespace Domain.Bans;

public static class BanErrors
{
    public static readonly Error CurrentTimeEarlierThanDateOfUnban = new(
        "Ban.CurrentTimeEarlierThanDateOfUnban",
        "The current time is earlier than the specified date of unban.");
}