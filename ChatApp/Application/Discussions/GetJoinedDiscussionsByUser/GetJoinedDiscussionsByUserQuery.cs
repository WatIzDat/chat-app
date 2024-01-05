using Application.Abstractions.Messaging;

namespace Application.Discussions.GetJoinedDiscussionsByUser;

public sealed record GetJoinedDiscussionsByUserQuery(Guid UserId) : IQuery<List<DiscussionResponse>>;
