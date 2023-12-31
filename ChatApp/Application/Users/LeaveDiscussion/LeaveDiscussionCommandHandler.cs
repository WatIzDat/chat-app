using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.LeaveDiscussion;

internal sealed class LeaveDiscussionCommandHandler(IUserRepository userRepository)
    : ICommandHandler<LeaveDiscussionCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(LeaveDiscussionCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        Result result = user.LeaveDiscussion(request.DiscussionId);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        userRepository.Update(user);

        return Result.Success();
    }
}
