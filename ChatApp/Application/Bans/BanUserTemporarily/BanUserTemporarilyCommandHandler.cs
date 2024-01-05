using Application.Abstractions.Messaging;
using Domain.Bans;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.Bans.BanUserTemporarily;

internal sealed class BanUserTemporarilyCommandHandler(
    IBanRepository banRepository,
    IUserRepository userRepository,
    IDiscussionRepository discussionRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<BanUserTemporarilyCommand, Guid>
{
    private readonly IBanRepository banRepository = banRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDiscussionRepository discussionRepository = discussionRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;

    public async Task<Result<Guid>> Handle(BanUserTemporarilyCommand request, CancellationToken cancellationToken)
    {
        if (await banRepository.BanExistsByUserAndDiscussionIdAsync(request.UserId, request.DiscussionId, cancellationToken))
        {
            return Result.Failure<Guid>(BanErrors.UserAlreadyBannedFromDiscussion);
        }

        if (!await userRepository.UserExistsAsync(request.UserId, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        if (!await discussionRepository.DiscussionExistsAsync(request.DiscussionId, cancellationToken))
        {
            return Result.Failure<Guid>(DiscussionErrors.NotFound);
        }

        Result<BanDetails> banDetailsResult = BanDetails.CreateTemporaryBan(dateTimeOffsetProvider.UtcNow, request.DateOfUnbanUtc);

        if (banDetailsResult.IsFailure)
        {
            return Result.Failure<Guid>(banDetailsResult.Error);
        }

        BanDetails banDetails = banDetailsResult.Value;

        Ban ban = Ban.Create(
            request.UserId,
            request.DiscussionId,
            dateTimeOffsetProvider.UtcNow,
            banDetails);

        banRepository.Insert(ban);

        return Result.Success(ban.Id);
    }
}

