using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.DeleteUserByClerkId;

internal sealed class DeleteUserByClerkIdCommandHandler(IUserRepository userRepository)
    : ICommandHandler<DeleteUserByClerkIdCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(DeleteUserByClerkIdCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByClerkIdAsync(request.ClerkId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        user.Delete();

        userRepository.Update(user);

        return Result.Success();
    }
}
