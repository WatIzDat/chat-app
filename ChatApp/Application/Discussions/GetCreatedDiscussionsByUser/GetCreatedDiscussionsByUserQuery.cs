using Application.Abstractions.Messaging;

namespace Application.Discussions.GetCreatedDiscussionsByUser;

public sealed record GetCreatedDiscussionsByUserQuery(Guid UserId) : IQuery<List<DiscussionResponse>>;
