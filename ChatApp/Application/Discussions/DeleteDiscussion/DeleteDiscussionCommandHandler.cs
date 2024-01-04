using Application.Abstractions.Messaging;
using Domain.Discussions;
using SharedKernel;

namespace Application.Discussions.DeleteDiscussion;

internal sealed class DeleteDiscussionCommandHandler(IDiscussionRepository discussionRepository)
    : ICommandHandler<DeleteDiscussionCommand>
{
    private readonly IDiscussionRepository discussionRepository = discussionRepository;

    public async Task<Result> Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
    {
        Discussion? discussion = await discussionRepository.GetByIdAsync(request.DiscussionId, cancellationToken);

        if (discussion == null)
        {
            return Result.Failure(DiscussionErrors.NotFound);
        }

        discussion.Delete();

        discussionRepository.Update(discussion);

        return Result.Success();
    }
}

