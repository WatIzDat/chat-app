using Application.Abstractions.Messaging;

namespace Application.Users.GetUsersByDiscussionIdAndRoleId;

public sealed record GetUsersByDiscussionIdAndRoleIdQuery(Guid DiscussionId, Guid RoleId, int Limit) : IQuery<List<UserResponse>>;
