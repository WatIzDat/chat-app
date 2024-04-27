using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false, cancellationToken);
    }

    public async Task<User?> GetByClerkIdAsync(string clerkId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ClerkId == clerkId && u.IsDeleted == false, cancellationToken);
    }

    public void Insert(User user)
    {
        dbContext.Set<User>().Add(user);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        bool exists = await dbContext.Users
            .AnyAsync(u => u.Email == email && u.IsDeleted == false, cancellationToken);

        // Unique means not existing, so negation is done
        return !exists;
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        bool exists = await dbContext.Users
            .AnyAsync(u => u.Username == username && u.IsDeleted == false, cancellationToken);

        // Unique means not existing, so negation is done
        return !exists;
    }

    public void Update(User user)
    {
        dbContext.Set<User>().Update(user);
    }

    public async Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Id == id && u.IsDeleted == false, cancellationToken);
    }
}
