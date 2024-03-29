using Domain.Messages;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class MessageRepository(ApplicationDbContext dbContext) : IMessageRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public void Delete(Message message)
    {
        throw new NotImplementedException();
    }

    public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Insert(Message message)
    {
        throw new NotImplementedException();
    }

    public void Update(Message message)
    {
        throw new NotImplementedException();
    }
}
