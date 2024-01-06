namespace Domain.Bans;

public interface IBanRepository
{
    void Insert(Ban ban);

    void Update(Ban ban);

    Task<Ban?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> BanExistsByUserAndDiscussionIdAsync(Guid userId, Guid discussionId, CancellationToken cancellationToken = default);
}
