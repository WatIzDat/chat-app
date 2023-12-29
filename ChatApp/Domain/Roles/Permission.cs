using SharedKernel;

namespace Domain.Roles;

public sealed record Permission
{
    public static readonly Permission BanUser = new("user:ban");
    public static readonly Permission KickUser = new("user:kick");
    public static readonly Permission MuteUser = new("user:mute");
    public static readonly Permission DeleteMessage = new("message:delete");

    public static readonly List<Permission> AllowedPermissions =
    [
        BanUser,
        KickUser,
        MuteUser,
        DeleteMessage
    ];

    private Permission(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Permission> Create(string value)
    {
        Permission permission = new(value);

        if (!AllowedPermissions.Contains(permission))
        {
            return Result.Failure<Permission>(PermissionErrors.NotAllowed);
        }

        return Result.Success(permission);
    }
}