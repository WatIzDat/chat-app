using Application.Abstractions.Messaging;

namespace Application.Roles.EditName;

public sealed record EditNameCommand(Guid RoleId, string Name) : ICommand;
