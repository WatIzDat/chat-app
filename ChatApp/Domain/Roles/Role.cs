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
        if (ListUtility.HasDuplicates(permissions))
        {
            return Result.Failure<Role>(RoleErrors.DuplicatePermissions);
        }

        Role role = new(Guid.NewGuid(), discussionId, name, permissions);

        return Result.Success(role);
    }
}
