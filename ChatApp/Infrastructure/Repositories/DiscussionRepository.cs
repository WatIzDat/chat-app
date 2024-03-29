using Domain.Discussions;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class DiscussionRepository(ApplicationDbContext dbContext) : IDiscussionRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public Task<bool> DiscussionExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Discussion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Insert(Discussion discussion)
    {
        throw new NotImplementedException();
    }

    public void Update(Discussion discussion)
    {
        throw new NotImplementedException();
    }
}
