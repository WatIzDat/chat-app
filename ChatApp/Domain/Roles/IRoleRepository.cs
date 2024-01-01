using Domain.Users;

namespace Domain.Roles;

public interface IRoleRepository
{
    void Insert(Role role);

    Task<bool> RoleExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> RoleInDiscussionsListAsync(Guid id, DiscussionsList discussions, CancellationToken cancellationToken = default);

    Task<bool> DuplicateRoleNamesInDiscussionAsync(string roleName, Guid discussionId, CancellationToken cancellationToken = default);
}
