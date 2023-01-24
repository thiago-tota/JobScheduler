using JobScheduler.Core;

namespace JobScheduler.Server.Jobs
{
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; } = string.Empty;
        public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
    }
}
