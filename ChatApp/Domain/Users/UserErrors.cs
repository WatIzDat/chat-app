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

    public static readonly Error EmailNotUnique = new(
        "User.EmailNotUnique",
        "A user with the requested email is already in the database.");

    public static readonly Error NotFound = new(
        "User.NotFound",
        "The user with the requested ID was not found.");
}