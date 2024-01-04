using Application.Abstractions.Messaging;

namespace Application.Discussions.CreateDiscussion;

public sealed record CreateDiscussionCommand(Guid UserId, string Name) : ICommand<Guid>;
