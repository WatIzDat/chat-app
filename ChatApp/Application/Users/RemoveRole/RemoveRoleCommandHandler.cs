using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.RemoveRole;

internal sealed class RemoveRoleCommandHandler(IUserRepository userRepository)
    : ICommandHandler<RemoveRoleCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        Result result = user.RemoveRole(request.RoleId);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        userRepository.Update(user);

        return Result.Success();
    }
}
