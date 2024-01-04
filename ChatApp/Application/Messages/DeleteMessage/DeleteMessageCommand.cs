using Application.Abstractions.Messaging;

namespace Application.Messages.DeleteMessage;

public sealed record DeleteMessageCommand(Guid MessageId) : ICommand;
