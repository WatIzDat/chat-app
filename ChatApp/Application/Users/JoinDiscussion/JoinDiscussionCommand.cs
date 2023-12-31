using Application.Abstractions.Messaging;

namespace Application.Users.JoinDiscussion;

public sealed record JoinDiscussionCommand(Guid UserId, Guid DiscussionId) : ICommand;
