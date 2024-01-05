namespace Application.Discussions;

public sealed record DiscussionResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
