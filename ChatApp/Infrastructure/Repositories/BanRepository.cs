using Domain.Bans;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class BanRepository(ApplicationDbContext dbContext) : IBanRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<bool> BanExistsByUserAndDiscussionIdAsync(Guid userId, Guid discussionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Bans
            .AnyAsync(b => b.UserId == userId &&
                           b.DiscussionId == discussionId &&
                           b.IsUnbanned == false,
                           cancellationToken);
    }

    public async Task<Ban?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Bans
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public void Insert(Ban ban)
    {
        dbContext.Set<Ban>().Add(ban);
    }

    public void Update(Ban ban)
    {
        dbContext.Set<Ban>().Update(ban);
    }
}
