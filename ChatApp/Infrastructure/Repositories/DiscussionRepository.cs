using Domain.Discussions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class DiscussionRepository(ApplicationDbContext dbContext) : IDiscussionRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<bool> DiscussionExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Discussions
            .AnyAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Discussion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Discussions
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Insert(Discussion discussion)
    {
        dbContext.Set<Discussion>().Add(discussion);
    }

    public void Update(Discussion discussion)
    {
        dbContext.Set<Discussion>().Update(discussion);
    }
}
