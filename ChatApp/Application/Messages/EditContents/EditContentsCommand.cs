using Application.Abstractions.Messaging;

namespace Application.Messages.EditContents;

public sealed record EditContentsCommand(Guid MessageId, string Contents) : ICommand;
