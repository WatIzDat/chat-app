using SharedKernel;

namespace Domain.Roles;

public class PermissionErrors
{
    public static readonly Error NotAllowed = Error.Validation(
        "Permission.NotAllowed",
        "The provided permission is not in the list of allowed permissions.");
}