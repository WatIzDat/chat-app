using Application.Abstractions.Messaging;
using Domain.Bans;
using Domain.Discussions;
using Domain.Messages;
using Domain.Users;
using SharedKernel;

namespace Application.Messages.SendMessage;

internal sealed class SendMessageCommandHandler(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    IDiscussionRepository discussionRepository,
    IBanRepository banRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<SendMessageCommand, Guid>
{
    private readonly IMessageRepository messageRepository = messageRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDiscussionRepository discussionRepository = discussionRepository;
    private readonly IBanRepository banRepository = banRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;

    public async Task<Result<Guid>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.UserExistsAsync(request.UserId, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        if (!await discussionRepository.DiscussionExistsAsync(request.DiscussionId, cancellationToken))
        {
            return Result.Failure<Guid>(DiscussionErrors.NotFound);
        }

        if (await banRepository.BanExistsByUserAndDiscussionIdAsync(
            request.UserId,
            request.DiscussionId,
            cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.UserBannedFromDiscussion);
        }
        
        Result<Message> messageResult = Message.Create(
            request.UserId,
            request.DiscussionId,
            request.Contents,
            dateTimeOffsetProvider.UtcNow);

        if (messageResult.IsFailure)
        {
            return Result.Failure<Guid>(messageResult.Error);
        }

        Message message = messageResult.Value;

        messageRepository.Insert(message);

        return Result.Success(message.Id);
    }
}
