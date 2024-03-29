using SharedKernel;

namespace Domain.Users;

public class DiscussionsListErrors
{
    public static readonly Error TooManyDiscussions = Error.Validation(
        "DiscussionsList.TooManyDiscussions",
        $"A user cannot be in more than {DiscussionsList.MaxLength} discussions.");

    public static readonly Error DuplicateDiscussions = Error.Conflict(
        "DiscussionsList.DuplicateDiscussions",
        "A user cannot be in multiple of the same discussion.");
}