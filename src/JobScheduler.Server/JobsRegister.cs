using JobScheduler.Core;
using JobScheduler.Server.Jobs;
using Microsoft.Extensions.Configuration;

namespace JobScheduler.Server
{
    public static class JobsRegister
    {
        public static void AddJobs(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.Configure<KeyValuePair<string, string>>(configurationManager.GetSection("JobSchedules"));

            var jobSchedules = configurationManager.GetSection(JobSchedulesDto.JobSchedules).Get<KeyValuePair<string, string>>();

            services.AddCronJob<MyCronFirstJob>(c => c.CronExpression = @"");
        }

        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);

            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
            return services;
        }
    }
}
