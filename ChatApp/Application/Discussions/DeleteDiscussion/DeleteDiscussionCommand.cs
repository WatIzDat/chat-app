using Application.Abstractions.Messaging;

namespace Application.Discussions.DeleteDiscussion;

public sealed record DeleteDiscussionCommand(Guid DiscussionId) : ICommand;
