using Application.Abstractions.Messaging;

namespace Application.Discussions.EditName;

public sealed record EditNameCommand(Guid DiscussionId, string Name) : ICommand;
