using Domain.Roles;
using Domain.Users;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public void Delete(Role role)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DuplicateRoleNamesInDiscussionAsync(string roleName, Guid discussionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Insert(Role role)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RoleExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RoleInDiscussionsListAsync(Guid id, DiscussionsList discussions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(Role role)
    {
        throw new NotImplementedException();
    }
}
