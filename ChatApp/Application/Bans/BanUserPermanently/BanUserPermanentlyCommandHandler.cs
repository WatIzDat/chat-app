using Application.Abstractions.Messaging;
using Domain.Bans;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.Bans.BanUserPermanently;

internal sealed class BanUserPermanentlyCommandHandler(
    IBanRepository banRepository,
    IUserRepository userRepository,
    IDiscussionRepository discussionRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<BanUserPermanentlyCommand, Guid>
{
    private readonly IBanRepository banRepository = banRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDiscussionRepository discussionRepository = discussionRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;

    public async Task<Result<Guid>> Handle(BanUserPermanentlyCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.UserExistsAsync(request.UserId))
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        if (!await discussionRepository.DiscussionExistsAsync(request.DiscussionId))
        {
            return Result.Failure<Guid>(DiscussionErrors.NotFound);
        }

        BanDetails banDetails = BanDetails.CreatePermanentBan();

        Ban ban = Ban.Create(
            request.UserId,
            request.DiscussionId,
            dateTimeOffsetProvider.UtcNow,
            banDetails);

        banRepository.Insert(ban);

        return Result.Success(ban.Id);
    }
}
