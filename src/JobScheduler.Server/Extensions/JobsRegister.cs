using JobScheduler.Server.JobBase;
using JobScheduler.Server.Jobs;

namespace JobScheduler.Server.Extensions
{
    public static class JobsRegister
    {
        public static void AddJobs(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            const string JobSchedules = "JobSchedules";
            IEnumerable<IConfigurationSection> jobSchedules = configurationManager.GetSection(JobSchedules).GetChildren();

            services.AddCronJob<MyFirstCronJob>(c =>
            {
                c.CronExpression = jobSchedules.GetConfigValue(nameof(MyFirstCronJob));
                c.TimeZoneInfo = TimeZoneInfo.Local;
            });

            services.AddCronJob<MySecondCronJob>(c =>
            {
                c.CronExpression = jobSchedules.GetConfigValue(nameof(MySecondCronJob));
                c.TimeZoneInfo = TimeZoneInfo.Local;
            });
        }

        static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobServiceBase
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

        static string GetConfigValue(this IEnumerable<IConfigurationSection> configurationSections, string configKey)
        {
            return configurationSections.Where(f => f.Key == configKey).FirstOrDefault()?.Value ?? "";
        }
    }
}
