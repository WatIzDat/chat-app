using Application.Abstractions.Messaging;

namespace Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : ICommand;
