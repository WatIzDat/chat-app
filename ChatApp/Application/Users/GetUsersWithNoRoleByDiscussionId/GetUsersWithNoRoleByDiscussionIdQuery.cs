using Application.Abstractions.Messaging;

namespace Application.Users.GetUsersWithNoRoleByDiscussionId;

public sealed record GetUsersWithNoRoleByDiscussionIdQuery : IQuery<List<UserResponse>>
{
    public Guid DiscussionId { get; init; }
    public DateTimeOffset LastDateCreatedUtc { get; init; }
    public Guid LastUserId { get; init; }
    public int Limit { get; init; }
}
