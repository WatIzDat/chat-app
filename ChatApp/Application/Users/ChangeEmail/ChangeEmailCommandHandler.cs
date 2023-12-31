using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.ChangeEmail;

internal sealed class ChangeEmailCommandHandler(IUserRepository userRepository)
    : ICommandHandler<ChangeEmailCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        Result<Email> emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        Email email = emailResult.Value;

        user.ChangeEmail(email);

        userRepository.Update(user);

        return Result.Success();
    }
}
