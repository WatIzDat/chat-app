using SharedKernel;

namespace Domain.Roles;

public static class RoleErrors
{
    public static readonly Error NameTooLong = Error.Validation(
        "Role.NameTooLong",
        $"The name cannot be longer than {Role.NameMaxLength} characters.");

    public static readonly Error DuplicatePermissions = Error.Conflict(
        "Role.DuplicatePermissions",
        "A role cannot have multiple of the same permission.");

    public static readonly Error NotFound = Error.NotFound(
        "Role.NotFound",
        "The role with the requested ID was not found.");

    public static readonly Error DuplicateRoleNamesInDiscussion = Error.Conflict(
        "Role.DuplicateRoleNamesInDiscussion",
        "A discussion cannot have multiple roles of the same name.");

    public static readonly Error PermissionNotFound = Error.NotFound(
        "Role.PermissionNotFound",
        "The requested permission was not found in the role's list of permissions.");

    public static readonly Error NotInDiscussion = Error.Validation(
        "Role.NotInDiscussion",
        "The requested role does not apply to the specified discussion.");
}
