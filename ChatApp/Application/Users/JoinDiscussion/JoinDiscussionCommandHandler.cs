using Application.Abstractions.Messaging;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.Users.JoinDiscussion;

internal sealed class JoinDiscussionCommandHandler(
    IUserRepository userRepository,
    IDiscussionRepository discussionRepository)
    : ICommandHandler<JoinDiscussionCommand>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDiscussionRepository discussionRepository = discussionRepository;

    public async Task<Result> Handle(JoinDiscussionCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (!await discussionRepository.DiscussionExistsAsync(request.DiscussionId, cancellationToken))
        {
            return Result.Failure(DiscussionErrors.NotFound);
        }

        if (await discussionRepository.DiscussionCreatedByUserAsync(
            request.DiscussionId,
            request.UserId,
            cancellationToken))
        {
            return Result.Failure(UserErrors.CannotJoinCreatedDiscussion);
        }

        Result result = user.JoinDiscussion(request.DiscussionId);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        userRepository.Update(user);

        return Result.Success();
    }
}
