namespace Domain.Discussions;

public interface IDiscussionRepository
{
    Task<bool> DiscussionExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
