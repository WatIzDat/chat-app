using SharedKernel;

namespace Domain.Discussions;

public static class DiscussionErrors
{
    public static readonly Error NameTooLong = new(
        "Discussion.NameTooLong",
        "The name cannot be longer than 50 characters.");
}