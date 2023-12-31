using Application.Abstractions.Messaging;

namespace Application.Users.AddRole;

public sealed record AddRoleCommand(Guid UserId, Guid RoleId) : ICommand;
