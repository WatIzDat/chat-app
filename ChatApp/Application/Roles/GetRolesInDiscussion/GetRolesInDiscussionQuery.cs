using Application.Abstractions.Messaging;

namespace Application.Roles.GetRolesInDiscussion;

public sealed record GetRolesInDiscussionQuery(Guid DiscussionId) : IQuery<List<RoleResponse>>;
