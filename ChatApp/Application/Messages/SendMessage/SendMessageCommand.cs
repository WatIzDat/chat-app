using Application.Abstractions.Messaging;

namespace Application.Messages.SendMessage;

public sealed record SendMessageCommand(Guid UserId, Guid DiscussionId, string Contents) : ICommand<Guid>;
