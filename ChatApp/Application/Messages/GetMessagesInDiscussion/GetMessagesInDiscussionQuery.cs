using Application.Abstractions.Messaging;

namespace Application.Messages.GetMessagesInDiscussion;

public sealed record GetMessagesInDiscussionQuery(
    Guid DiscussionId,
    DateTimeOffset LastDateSentUtc,
    Guid LastMessageId,
    int Limit)
    : IQuery<List<MessageResponse>>;
