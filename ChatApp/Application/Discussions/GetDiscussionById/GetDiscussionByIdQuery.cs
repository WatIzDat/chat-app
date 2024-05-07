using Application.Abstractions.Messaging;

namespace Application.Discussions.GetDiscussionById;

public sealed record GetDiscussionByIdQuery(Guid Id) : IQuery<DiscussionResponse>;
