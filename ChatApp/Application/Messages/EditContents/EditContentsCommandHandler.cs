using Application.Abstractions.Messaging;
using Domain.Messages;
using SharedKernel;

namespace Application.Messages.EditContents;

internal sealed class EditContentsCommandHandler(IMessageRepository messageRepository)
    : ICommandHandler<EditContentsCommand>
{
    private readonly IMessageRepository messageRepository = messageRepository;

    public async Task<Result> Handle(EditContentsCommand request, CancellationToken cancellationToken)
    {
        Message? message = await messageRepository.GetByIdAsync(request.MessageId, cancellationToken);

        if (message == null)
        {
            return Result.Failure(MessageErrors.NotFound);
        }

        Result result = message.EditContents(request.Contents);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        messageRepository.Update(message);

        return Result.Success();
    }
}
