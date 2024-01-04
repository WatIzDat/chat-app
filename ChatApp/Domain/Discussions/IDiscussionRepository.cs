namespace Domain.Discussions;

public interface IDiscussionRepository
{
    void Insert(Discussion discussion);

    void Update(Discussion discussion);

    Task<Discussion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> DiscussionExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
