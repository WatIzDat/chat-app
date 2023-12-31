using Application.Abstractions.Messaging;

namespace Application.Users.RemoveRole;

public sealed record RemoveRoleCommand(Guid UserId, Guid RoleId) : ICommand;
