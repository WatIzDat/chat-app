namespace Application.Messages;

public sealed record MessageResponse
{
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public required string Contents { get; init; }

    public required DateTimeOffset DateSentUtc { get; init; }

    public required bool IsEdited { get; init; }
}
