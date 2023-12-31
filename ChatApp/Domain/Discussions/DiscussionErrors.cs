using SharedKernel;

namespace Domain.Discussions;

public static class DiscussionErrors
{
    public static readonly Error NameTooLong = new(
        "Discussion.NameTooLong",
        $"The name cannot be longer than {Discussion.NameMaxLength} characters.");

    public static readonly Error NotFound = new(
        "Discussion.NotFound",
        "The discussion with the requested ID was not found.");
}