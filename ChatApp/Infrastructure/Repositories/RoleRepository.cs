using Domain.Roles;
using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public void Delete(Role role)
    {
        dbContext.Set<Role>().Remove(role);
    }

    public async Task<bool> DuplicateRoleNamesInDiscussionAsync(string roleName, Guid discussionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AnyAsync(r => r.DiscussionId == discussionId && r.Name == roleName, cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public void Insert(Role role)
    {
        dbContext.Set<Role>().Add(role);
    }

    public async Task<bool> RoleExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> RoleInDiscussionsListAsync(Guid id, DiscussionsList discussions, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AnyAsync(r => discussions.Value.Any(d => r.DiscussionId == d), cancellationToken);
    }

    public void Update(Role role)
    {
        dbContext.Set<Role>().Update(role);
    }
}
