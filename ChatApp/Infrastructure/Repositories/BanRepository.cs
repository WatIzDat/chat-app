using Domain.Bans;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class BanRepository(ApplicationDbContext dbContext) : IBanRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public Task<bool> BanExistsByUserAndDiscussionIdAsync(Guid userId, Guid discussionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Ban?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Insert(Ban ban)
    {
        throw new NotImplementedException();
    }

    public void Update(Ban ban)
    {
        throw new NotImplementedException();
    }
}
