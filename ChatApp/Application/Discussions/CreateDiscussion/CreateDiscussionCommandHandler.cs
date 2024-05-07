using Application.Abstractions.Messaging;
using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Application.Discussions.CreateDiscussion;

internal sealed class CreateDiscussionCommandHandler(
    IUserRepository userRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider,
    DiscussionService discussionService)
    : ICommandHandler<CreateDiscussionCommand, Guid>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;
    private readonly DiscussionService discussionService = discussionService;

    public async Task<Result<Guid>> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add limit to amount of discussions a single user can create

        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
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

        Result result = discussionService.CreateDiscussion(discussion, user);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        return Result.Success(discussion.Id);
    }
}

