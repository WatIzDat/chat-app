using SharedKernel;

namespace Domain.Roles;

public static class RoleErrors
{
    public static readonly Error NameTooLong = new(
        "Role.NameTooLong",
        $"The name cannot be longer than {Role.NameMaxLength} characters.");

    public static readonly Error DuplicatePermissions = new(
        "Role.DuplicatePermissions",
        "A role cannot have multiple of the same permission.");

    public static readonly Error NotFound = new(
        "Role.NotFound",
        "The role with the requested ID was not found.");

    public static readonly Error DuplicateRoleNamesInDiscussion = new(
        "Role.DuplicateRoleNamesInDiscussion",
        "A discussion cannot have multiple roles of the same name.");

    public static readonly Error PermissionNotFound = new(
        "Role.PermissionNotFound",
        "The requested permission was not found in the role's list of permissions.");
}
