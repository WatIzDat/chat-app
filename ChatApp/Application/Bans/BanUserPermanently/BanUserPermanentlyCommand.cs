using Application.Abstractions.Messaging;

namespace Application.Bans.BanUser;

public sealed record BanUserPermanentlyCommand(Guid UserId, Guid DiscussionId) : ICommand<Guid>;
