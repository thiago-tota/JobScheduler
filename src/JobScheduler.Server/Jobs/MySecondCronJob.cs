using JobScheduler.Server.JobBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JobScheduler.Server.Jobs
{
    public class MySecondCronJob : CronJobServiceBase
    {
        protected override string JobName => nameof(MySecondCronJob);
        private readonly ILogger<MySecondCronJob> _logger;

        public MySecondCronJob(IScheduleConfig<MySecondCronJob> config, ILogger<MySecondCronJob> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider, config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{now} MySecondCronJob is working.", DateTime.Now.ToString("T"));
            return Task.CompletedTask;
        }
    }
}
