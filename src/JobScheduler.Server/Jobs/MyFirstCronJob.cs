using JobScheduler.Server.JobBase;

namespace JobScheduler.Server.Jobs
{
    public class MyFirstCronJob : CronJobServiceBase
    {
        protected override string JobName => nameof(MyFirstCronJob);

        private readonly ILogger<MyFirstCronJob> _logger;

        public MyFirstCronJob(IScheduleConfig<MyFirstCronJob> config, ILogger<MyFirstCronJob> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider, config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{now} MyFirstCronJob is working.", DateTime.Now.ToString("T"));
            return Task.CompletedTask;
        }
    }
}
