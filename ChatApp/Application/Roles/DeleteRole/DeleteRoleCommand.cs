using Application.Abstractions.Messaging;

namespace Application.Roles.DeleteRole;

public sealed record DeleteRoleCommand(Guid RoleId) : ICommand;
