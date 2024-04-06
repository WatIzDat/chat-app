using Application.Abstractions.Messaging;

namespace Application.Bans.BanUserTemporarily;

public sealed record BanUserTemporarilyCommand(Guid UserId, Guid DiscussionId, DateTimeOffset DateWillBeUnbannedUtc) : ICommand<Guid>;
