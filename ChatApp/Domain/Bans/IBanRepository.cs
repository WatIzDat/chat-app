namespace Domain.Bans;

public interface IBanRepository
{
    void Insert(Ban ban);

    Task<bool> BanExistsByUserAndDiscussionIdAsync(Guid userId, Guid discussionId, CancellationToken cancellationToken = default);
}
