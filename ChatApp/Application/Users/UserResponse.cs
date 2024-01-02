namespace Application.Users;

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public DateTimeOffset DateCreatedUtc { get; init; }

    public string AboutSection { get; init; } = string.Empty;
}
