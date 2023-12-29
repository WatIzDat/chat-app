using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error UsernameTooLong = new(
        "User.UsernameTooLong",
        $"The username cannot be longer than {User.UsernameMaxLength} characters.");

    public static readonly Error DiscussionNotFound = new(
        "User.DiscussionNotFound",
        "The requested discussion was not found in the user's joined discussions.");

    public static readonly Error RoleNotFound = new(
        "User.RoleNotFound",
        "The requested role was not found in the user's roles.");
}