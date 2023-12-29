using SharedKernel;

namespace Domain.Users;

public class RolesListErrors
{
    public static readonly Error DuplicateRoles = new(
        "RolesList.DuplicateRoles",
        "A user cannot have multiple of the same role.");

    public static readonly Error TooManyRoles = new(
        "RolesList.TooManyRoles",
        "A user cannot have more roles than the amount of discussions they are in.");
}