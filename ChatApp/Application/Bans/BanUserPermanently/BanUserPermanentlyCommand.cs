using Application.Abstractions.Messaging;

namespace Application.Bans.BanUserPermanently;

public sealed record BanUserPermanentlyCommand(Guid UserId, Guid DiscussionId) : ICommand<Guid>;
