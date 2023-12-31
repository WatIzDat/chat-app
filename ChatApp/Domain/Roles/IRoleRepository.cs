using Domain.Users;

namespace Domain.Roles;

public interface IRoleRepository
{
    void Insert(Role role);

    void Update(Role role);

    void Delete(Role role);

    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> RoleExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> RoleInDiscussionsListAsync(Guid id, DiscussionsList discussions, CancellationToken cancellationToken = default);

    Task<bool> DuplicateRoleNamesInDiscussionAsync(string roleName, Guid discussionId, CancellationToken cancellationToken = default);
}
