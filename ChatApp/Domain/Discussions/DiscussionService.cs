using Domain.Users;
using SharedKernel;

namespace Domain.Discussions;

public sealed class DiscussionService(IDiscussionRepository discussionRepository, IUserRepository userRepository)
{
    private readonly IDiscussionRepository discussionRepository = discussionRepository;
    private readonly IUserRepository userRepository = userRepository;

    public Result CreateDiscussion(Discussion discussion, User userCreatedBy)
    {
        Result joinDiscussionResult = userCreatedBy.JoinDiscussion(discussion.Id);

        if (joinDiscussionResult.IsFailure)
        {
            return Result.Failure(joinDiscussionResult.Error);
        }

        userRepository.Update(userCreatedBy);

        discussionRepository.Insert(discussion);

        return Result.Success();
    }
}
