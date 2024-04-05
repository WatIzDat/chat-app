using Application.Abstractions.Data;
using Domain.Bans;
using Microsoft.EntityFrameworkCore;
using Quartz;
using SharedKernel;

namespace Infrastructure.BackgroundJobs;

public class UnbanUserBackgroundJob(
    IApplicationDbContext dbContext,
    IDateTimeOffsetProvider dateTimeOffsetProvider,
    IUnitOfWork unitOfWork)
    : IJob
{
    private readonly IApplicationDbContext dbContext = dbContext;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task Execute(IJobExecutionContext context)
    {
        List<Ban> bans = await dbContext.Bans
            .Where(b => b.BanDetails.DateWillBeUnbannedUtc < dateTimeOffsetProvider.UtcNow)
            .ToListAsync();

        bans.ForEach(b => b.UnbanUser());

        await unitOfWork.SaveChangesAsync();
    }
}
