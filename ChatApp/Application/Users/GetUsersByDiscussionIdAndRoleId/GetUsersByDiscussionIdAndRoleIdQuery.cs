using Application.Abstractions.Messaging;

namespace Application.Users.GetUsersByDiscussionIdAndRoleId;

public sealed record GetUsersByDiscussionIdAndRoleIdQuery : IQuery<List<UserResponse>>
{
    public Guid DiscussionId { get; init; }
    public Guid RoleId { get; init; }
    public DateTimeOffset LastDateCreatedUtc { get; init; }
    public Guid LastUserId { get; init; }
    public int Limit { get; init; }
}
