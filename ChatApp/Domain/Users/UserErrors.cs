using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error UsernameTooLong = new(
        "User.UsernameTooLong",
        "The username cannot be longer than 20 characters.");

    public static readonly Error AboutSectionTooLong = new(
        "User.AboutSectionTooLong",
        "The about section cannot be longer than 200 characters.");

    public static readonly Error TooManyDiscussions = new(
        "User.TooManyDiscussions",
        "A user cannot be in more than 100 discussions.");

    public static readonly Error TooManyRoles = new(
        "User.TooManyRoles",
        "A user cannot have more roles than the amount of discussions they are in.");

    public static readonly Error DuplicateDiscussions = new(
        "User.DuplicateDiscussions",
        "A user cannot be in multiple of the same discussion.");

    public static readonly Error DuplicateRoles = new(
        "User.DuplicateRoles",
        "A user cannot have multiple of the same role.");
}