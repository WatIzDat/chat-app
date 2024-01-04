namespace Domain.Discussions;

public interface IDiscussionRepository
{
    void Insert(Discussion discussion);

    Task<bool> DiscussionExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
