using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public class UnbanUserBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        JobKey jobKey = JobKey.Create(nameof(UnbanUserBackgroundJob));

        options
            .AddJob<UnbanUserBackgroundJob>(jobBuilder => 
                jobBuilder
                    .WithIdentity(jobKey)
                    .StoreDurably(true))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(5)
                                .RepeatForever()));

    }
}
