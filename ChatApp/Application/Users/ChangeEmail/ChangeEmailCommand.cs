using Application.Abstractions.Messaging;

namespace Application.Users.ChangeEmail;

public sealed record ChangeEmailCommand(Guid UserId, string Email) : ICommand;
