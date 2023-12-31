using Application.Abstractions.Messaging;

namespace Application.Users.ChangeUsername;

public sealed record ChangeUsernameCommand(Guid UserId, string Username) : ICommand;
