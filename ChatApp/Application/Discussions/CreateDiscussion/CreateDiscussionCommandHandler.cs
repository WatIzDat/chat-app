using Application.Abstractions.Messaging;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.Discussions.CreateDiscussion;

internal sealed class CreateDiscussionCommandHandler(
    IDiscussionRepository discussionRepository,
    IUserRepository userRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<CreateDiscussionCommand, Guid>
{
    private readonly IDiscussionRepository discussionRepository = discussionRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;

    public async Task<Result<Guid>> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add limit to amount of discussions a single user can create

        if (!await userRepository.UserExistsAsync(request.UserId))
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        Result<Discussion> discussionResult = Discussion.Create(
            request.UserId,
            request.Name,
            dateTimeOffsetProvider.UtcNow);

        if (discussionResult.IsFailure)
        {
            return Result.Failure<Guid>(discussionResult.Error);
        }

        Discussion discussion = discussionResult.Value;

        discussionRepository.Insert(discussion);

        return Result.Success(discussion.Id);
    }
}

