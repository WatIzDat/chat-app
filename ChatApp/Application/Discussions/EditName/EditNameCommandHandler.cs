using Application.Abstractions.Messaging;
using Domain.Discussions;
using SharedKernel;

namespace Application.Discussions.EditName;

internal sealed class EditNameCommandHandler(IDiscussionRepository discussionRepository) : ICommandHandler<EditNameCommand>
{
    private readonly IDiscussionRepository discussionRepository = discussionRepository;

    public async Task<Result> Handle(EditNameCommand request, CancellationToken cancellationToken)
    {
        Discussion? discussion = await discussionRepository.GetByIdAsync(request.DiscussionId, cancellationToken);

        if (discussion == null)
        {
            return Result.Failure(DiscussionErrors.NotFound);
        }

        Result result = discussion.EditName(request.Name);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        discussionRepository.Update(discussion);

        return Result.Success();
    }
}

