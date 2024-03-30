using Application.Abstractions.Messaging;

namespace Application.Users.GetUsersWithNoRoleByDiscussionId;

public sealed record GetUsersWithNoRoleByDiscussionIdQuery : IQuery<List<UserResponse>>
{
    public GetUsersWithNoRoleByDiscussionIdQuery(
        Guid discussionId,
        DateTimeOffset lastDateCreatedUtc,
        Guid lastUserId,
        int limit)
    {
        DiscussionId = discussionId;
        LastDateCreatedUtc = lastDateCreatedUtc;
        LastUserId = lastUserId;
        Limit = limit;
    }

    public Guid DiscussionId { get; init; }
    public DateTimeOffset LastDateCreatedUtc { get; init; }
    public Guid LastUserId { get; init; }
    public int Limit { get; init; }
}
