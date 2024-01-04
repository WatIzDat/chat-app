using Application.Abstractions.Messaging;
using Domain.Messages;
using SharedKernel;

namespace Application.Messages.SendMessage;

internal sealed class SendMessageCommandHandler(
    IMessageRepository messageRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<SendMessageCommand, Guid>
{
    public Task<Result<Guid>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add check for user and discussion exists
        
        Result<Message> messageResult = Message.Create(
            request.UserId,
            request.DiscussionId,
            request.Contents,
            dateTimeOffsetProvider.UtcNow);

        if (messageResult.IsFailure)
        {
            return Task.FromResult(Result.Failure<Guid>(messageResult.Error));
        }

        Message message = messageResult.Value;

        messageRepository.Insert(message);

        return Task.FromResult(Result.Success(message.Id));
    }
}

