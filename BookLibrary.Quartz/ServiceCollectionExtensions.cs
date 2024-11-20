using Microsoft.Extensions.DependencyInjection;
using Quartz;
using BookLibrary.Quartz.Jobs;
using BookLibrary.Quartz.Services;

namespace BookLibrary.Quartz;

public static class ServiceCollectionExtensions
{
    public static void AddQuartz(this IServiceCollection services)
    {
        services.AddScoped<DailyJob>();
        services.AddScoped<IEmailService, FakeEmailService>();
        services.AddQuartz(_ =>
        {
            _.UseInMemoryStore();

            _.AddJob<DailyJob>(opts => opts.WithIdentity("dailyJob"));
            _.AddTrigger(opts => opts
                .ForJob("dailyJob")
                .WithIdentity("dailyTrigger")
                .StartNow()
                .WithSimpleSchedule(b => b
                    .WithInterval(TimeSpan.FromDays(1))
                    .RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}