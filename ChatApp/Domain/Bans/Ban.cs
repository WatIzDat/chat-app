using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Domain.Bans;

public sealed class Ban : Entity
{
    // Private parameterless constructor for EF
    private Ban()
    {
    }
    
    private Ban(
        Guid id,
        Guid userId,
        Guid discussionId,
        DateTimeOffset dateOfBanUtc,
        BanDetails banDetails,
        bool isUnbanned)
        : base(id)
    {
        UserId = userId;
        DiscussionId = discussionId;
        DateOfBanUtc = dateOfBanUtc;
        BanDetails = banDetails;
        IsUnbanned = isUnbanned;
    }

    public Guid UserId { get; private set; }

    public Guid DiscussionId { get; private set; }

    public DateTimeOffset DateOfBanUtc { get; private set; }

    public BanDetails BanDetails { get; private set; }

    public bool IsUnbanned { get; private set; }

    // Navigation property for EF
    public User UserNavigation { get; set; } = null!;

    // Navigation property for EF
    public Discussion DiscussionNavigation { get; set; } = null!;

    public static Ban Create(
        Guid userId,
        Guid discussionId,
        DateTimeOffset dateOfBanUtc,
        BanDetails banDetails)
    {
        Ban ban = new(Guid.NewGuid(), userId, discussionId, dateOfBanUtc, banDetails, isUnbanned: false);

        return ban;
    }

    public Result UnbanUser()
    {
        IsUnbanned = true;

        return Result.Success();
    }
}
