namespace Application.Users;

public sealed record UserResponse
{
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public required string Email { get; init; }

    public required DateTimeOffset DateCreatedUtc { get; init; }

    public required string AboutSection { get; init; }
}
