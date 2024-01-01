using Application.Abstractions.Messaging;

namespace Application.Roles.CreateRole;

public sealed record CreateRoleCommand(Guid DiscussionId, string Name, List<string> Permissions) : ICommand<Guid>;
