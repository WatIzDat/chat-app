using Domain.Users;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Insert(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
