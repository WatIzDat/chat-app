﻿using SharedKernel;

namespace Domain.Bans;

public sealed class Ban : Entity
{
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

    public static Ban Create(
        Guid userId,
        Guid discussionId,
        DateTimeOffset dateOfBanUtc,
        BanDetails banDetails)
    {
        Ban ban = new(Guid.NewGuid(), userId, discussionId, dateOfBanUtc, banDetails, isUnbanned: false);

        return ban;
    }

    public Result UnbanUser(DateTimeOffset currentTime)
    {
        if (currentTime < BanDetails.DateOfUnbanUtc)
        {
            return Result.Failure(BanErrors.CurrentTimeEarlierThanDateOfUnban);
        }

        IsUnbanned = true;

        return Result.Success();
    }
}
