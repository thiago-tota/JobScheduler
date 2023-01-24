using JobScheduler.Core;

namespace JobScheduler.Server.Jobs
{
    public class MyCronFirstJob : CronJobService
    {
        private readonly ILogger<MyCronFirstJob> _logger;

        public MyCronFirstJob(IScheduleConfig<MyCronFirstJob> config, ILogger<MyCronFirstJob> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MyCronFirstJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{now} MyCronFirstJob 1 is working.", DateTime.Now.ToString("T"));
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MyCronFirstJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
