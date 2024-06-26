using Application.Abstractions.Messaging;
using Domain.Bans;
using SharedKernel;

namespace Application.Bans.UnbanUser;

internal sealed class UnbanUserCommandHandler(
    IBanRepository banRepository)
    : ICommandHandler<UnbanUserCommand>
{
    private readonly IBanRepository banRepository = banRepository;

    public async Task<Result> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
    {
        Ban? ban = await banRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (ban == null)
        {
            return Result.Failure(BanErrors.NotFoundByUserId);
        }

        Result result = ban.UnbanUser();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        banRepository.Update(ban);

        return Result.Success();
    }
}

