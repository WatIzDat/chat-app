using Application.Abstractions.Messaging;
using Domain.Bans;
using SharedKernel;

namespace Application.Bans.UnbanUser;

internal sealed class UnbanUserCommandHandler(
    IBanRepository banRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<UnbanUserCommand>
{
    private readonly IBanRepository banRepository = banRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;

    public async Task<Result> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
    {
        Ban? ban = await banRepository.GetByIdAsync(request.BanId, cancellationToken);

        if (ban == null)
        {
            return Result.Failure(BanErrors.NotFound);
        }

        Result result = ban.UnbanUser(dateTimeOffsetProvider.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        banRepository.Update(ban);

        return Result.Success();
    }
}

