using Domain.Messages;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MessageRepository(ApplicationDbContext dbContext) : IMessageRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public void Delete(Message message)
    {
        dbContext.Set<Message>().Remove(message);
    }

    public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public void Insert(Message message)
    {
        dbContext.Set<Message>().Add(message);
    }

    public void Update(Message message)
    {
        dbContext.Set<Message>().Update(message);
    }
}
