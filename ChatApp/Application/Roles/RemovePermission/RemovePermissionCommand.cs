using Application.Abstractions.Messaging;

namespace Application.Roles.RemovePermission;

public sealed record RemovePermissionCommand(Guid RoleId, string Permission) : ICommand;
