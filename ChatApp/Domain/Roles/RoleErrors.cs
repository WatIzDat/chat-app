using SharedKernel;

namespace Domain.Roles;

public static class RoleErrors
{
    public static readonly Error NameTooLong = new(
        "Role.NameTooLong",
        "The name cannot be longer than 20 characters.");

    public static readonly Error DuplicatePermissions = new(
        "Role.DuplicatePermissions",
        "A role cannot have multiple of the same permission.");
}