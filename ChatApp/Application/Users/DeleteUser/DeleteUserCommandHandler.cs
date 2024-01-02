using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.DeleteUser;

internal sealed class DeleteUserCommmandHandler(IUserRepository userRepository) : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        user.Delete();

        userRepository.Update(user);

        return Result.Success();
    }
}

