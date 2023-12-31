using Application.Abstractions.Messaging;

namespace Application.Users.LeaveDiscussion;

public sealed record LeaveDiscussionCommand(Guid UserId, Guid DiscussionId) : ICommand;
