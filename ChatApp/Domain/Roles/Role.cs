using SharedKernel;
using SharedKernel.Utility;

namespace Domain.Roles;

public sealed class Role : Entity
{
    private Role(
        Guid id,
        Guid discussionId,
        string name,
        List<Permission> permissions)
        : base(id)
    {
        DiscussionId = discussionId;
        Name = name;
        Permissions = permissions;
    }

    public Guid DiscussionId { get; private set; }

    public string Name { get; private set; }

    public List<Permission> Permissions { get; private set; }

    public static Result<Role> Create(
        Guid discussionId,
        string name,
        List<Permission> permissions)
    {
        if (name.Length > 20)
        {
            return Result.Failure<Role>(RoleErrors.NameTooLong);
        }

        if (ListUtility.HasDuplicates(permissions))
        {
            return Result.Failure<Role>(RoleErrors.DuplicatePermissions);
        }

        Role role = new(Guid.NewGuid(), discussionId, name, permissions);

        return Result.Success(role);
    }

    public Result EditName(string name)
    {
        if (name.Length > 20)
        {
            return Result.Failure<Role>(RoleErrors.NameTooLong);
        }

        Name = name;

        return Result.Success();
    }

    public Result AddPermission(Permission permission)
    {
        List<Permission> permissions = new(Permissions) 
        {
            permission
        };

        if (ListUtility.HasDuplicates(permissions))
        {
            return Result.Failure<Role>(RoleErrors.DuplicatePermissions);
        }

        Permissions.Add(permission);

        return Result.Success();
    }

    public void RemovePermission(Permission permission)
    {
        Permissions.Remove(permission);
    }
}
