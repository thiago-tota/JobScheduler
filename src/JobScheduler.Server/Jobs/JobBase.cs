using JobScheduler.Core.Enums;
using JobScheduler.Infraestructure.Data;

namespace JobScheduler.Server.Jobs
{
    public abstract class JobBase : BackgroundService
    {
        protected abstract string JobName { get; }

        private readonly ILogger<JobBase> _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly int? _intervalInSeconds;
        private readonly TimeOnly? _runTime;

        public JobBase(ILogger<JobBase> logger, IServiceProvider serviceProvider, int intervalInSeconds)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _intervalInSeconds = intervalInSeconds;
        }

        public JobBase(ILogger<JobBase> logger, IServiceProvider serviceProvider, TimeOnly runTime)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _runTime = runTime;
        }

        protected abstract Task JobAsync(CancellationToken stoppingToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if(_intervalInSeconds > 0)
            {
                await RunIntervalJob(stoppingToken);
            }
            if(_runTime != null)
            {
                await RunSpecificTimeJob(stoppingToken);
            }
        }

        private async Task RunIntervalJob(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunJob(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(_intervalInSeconds!.Value), stoppingToken);
            }
        }

        private async Task RunSpecificTimeJob(CancellationToken stoppingToken)
        {
            do
            {
                var delay = GetDelayUntilNextRun();
                await Task.Delay(delay, stoppingToken);
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                await RunJob(stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task RunJob(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Job {JobName} started at time {DateTime.Now}");
            var entity = new RunHistory
            {
                JobName = JobName,
                StartTime = DateTime.Now,
            };

            await JobAsync(stoppingToken);
            _logger.LogInformation($"Job {JobName} finished at time {DateTime.Now}");

            entity.EndTime = DateTime.Now;
            entity.Status = JobStatus.Run;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<JobSchedulerDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
                dbContext.RunHistory.Add(entity);
                await dbContext.SaveChangesAsync();
            }
            _logger.LogInformation($"Job {JobName} result written to DB at time {DateTime.Now}");
        }

        private TimeSpan GetDelayUntilNextRun()
        {
            var now = DateTime.Now;
            var nextExecution = new DateTime(now.Year, now.Month, now.Day, _runTime?.Hour ?? 0, _runTime?.Minute ?? 0, _runTime?.Second ?? 0);
            if (nextExecution > now)
            {
                return nextExecution - now;
            }
            //Time is in past, so postpone to next day
            return nextExecution.AddDays(1) - now;
        }

        //protected override abstract Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
