using Application.Abstractions.Messaging;

namespace Application.Bans.UnbanUser;

public sealed record UnbanUserCommand(Guid UserId) : ICommand;
