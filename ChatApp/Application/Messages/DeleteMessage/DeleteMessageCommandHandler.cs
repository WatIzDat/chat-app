using Application.Abstractions.Messaging;
using Domain.Messages;
using SharedKernel;

namespace Application.Messages.DeleteMessage;

internal sealed class DeleteMessageCommandHandler(IMessageRepository messageRepository) : ICommandHandler<DeleteMessageCommand>
{
    private readonly IMessageRepository messageRepository = messageRepository;

    public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message = await messageRepository.GetByIdAsync(request.MessageId, cancellationToken);

        if (message == null)
        {
            return Result.Failure(MessageErrors.NotFound);
        }

        messageRepository.Delete(message);

        return Result.Success();
    }
}
