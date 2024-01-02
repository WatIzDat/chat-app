using Application.Abstractions.Messaging;

namespace Application.Roles.AddPermission;

public sealed record AddPermissionCommand(Guid RoleId, string Permission) : ICommand;
