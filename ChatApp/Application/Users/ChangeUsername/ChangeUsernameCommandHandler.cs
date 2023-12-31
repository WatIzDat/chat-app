using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.ChangeUsername;

internal sealed class ChangeUsernameCommandHandler(IUserRepository userRepository)
    : ICommandHandler<ChangeUsernameCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(ChangeUsernameCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        Result result = user.ChangeUsername(request.Username);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        userRepository.Update(user);

        return Result.Success();
    }
}
