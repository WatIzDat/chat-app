using SharedKernel;

namespace Domain.Discussions;

public static class DiscussionErrors
{
    public static readonly Error NameTooLong = Error.Validation(
        "Discussion.NameTooLong",
        $"The name cannot be longer than {Discussion.NameMaxLength} characters.");

    public static readonly Error NotFound = Error.NotFound(
        "Discussion.NotFound",
        "The discussion with the requested ID was not found.");
}