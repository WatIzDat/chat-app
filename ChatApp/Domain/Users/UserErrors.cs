using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error UsernameTooLong = Error.Validation(
        "User.UsernameTooLong",
        $"The username cannot be longer than {User.UsernameMaxLength} characters.");

    public static readonly Error DiscussionNotFound = Error.NotFound(
        "User.DiscussionNotFound",
        "The requested discussion was not found in the user's joined discussions.");

    public static readonly Error RoleNotFound = Error.NotFound(
        "User.RoleNotFound",
        "The requested role was not found in the user's roles.");

    public static readonly Error RoleNotInDiscussionsList = Error.Validation(
        "User.RoleNotInDiscussionsList",
        "The requested role does not apply to any of the user's joined discussions.");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "User.EmailNotUnique",
        "A user with the requested email is already in the database.");

    public static readonly Error UsernameNotUnique = Error.Conflict(
        "User.UsernameNotUnique",
        "A user with the requested username is already in the database.");

    public static readonly Error UserBannedFromDiscussion = Error.Forbidden(
        "User.UserBannedFromDiscussion",
        "The user cannot perform the requested action due to being banned from the discussion.");

    public static readonly Error CannotJoinCreatedDiscussion = Error.Validation(
        "User.CannotJoinCreatedDiscussion",
        "A user cannot join the user's own discussion.");

    public static readonly Error NotFound = Error.NotFound(
        "User.NotFound",
        "The user with the requested ID was not found.");
}
