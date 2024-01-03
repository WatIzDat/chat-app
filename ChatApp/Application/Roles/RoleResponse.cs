namespace Application.Roles;

public sealed record RoleResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required List<string> Permissions { get; init; }
}
