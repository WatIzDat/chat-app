using SharedKernel;

namespace Domain.Bans;

public static class BanErrors
{
    public static readonly Error CurrentTimeEarlierThanDateOfUnban = Error.Validation(
        "Ban.CurrentTimeEarlierThanDateOfUnban",
        "The current time is earlier than the specified date of unban.");

    public static readonly Error CurrentTimePastDateWillBeUnbanned = Error.Validation(
        "Ban.CurrentTimePastDateWillBeUnbanned",
        "The current time is past the specified date the user will be unbanned.");

    public static readonly Error UserAlreadyBannedFromDiscussion = Error.Conflict(
        "Ban.UserAlreadyBannedFromDiscussion",
        "The requested user has already been banned from the specified discussion.");

    public static readonly Error NotFoundByUserId = Error.NotFound(
        "Ban.NotFoundByUserId",
        "A ban with the requested user ID that has not been unbanned was not found.");

    public static readonly Error NotFound = Error.NotFound(
        "Ban.NotFound",
        "The ban with the requested ID was not found.");
}
