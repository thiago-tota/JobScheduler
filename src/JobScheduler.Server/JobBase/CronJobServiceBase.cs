using Cronos;
using JobScheduler.Core.Enums;
using JobScheduler.Infraestructure.Data;

namespace JobScheduler.Server.JobBase
{
    public abstract class CronJobServiceBase : IHostedService, IDisposable
    {
        private System.Timers.Timer? _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        private readonly ILogger<CronJobServiceBase> _logger;
        private readonly IServiceProvider _serviceProvider;

        protected abstract string JobName { get; }

        protected CronJobServiceBase(ILogger<CronJobServiceBase> logger, IServiceProvider serviceProvider, string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Job {JobName} is starting at time {DateTime.Now}.");
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (_, _) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ExecuteJob(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Job {JobName} is stopping at time {DateTime.Now}.");
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }

        private async Task ExecuteJob(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Job {JobName} execution started at time {DateTime.Now}");

            var entity = new RunHistory
            {
                JobName = JobName,
                StartTime = DateTime.Now,
            };

            await DoWork(cancellationToken);
            _logger.LogInformation($"Job {JobName} execution finished at time {DateTime.Now}");

            entity.EndTime = DateTime.Now;
            entity.Status = JobStatus.Run;

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<JobSchedulerDbContext>();
                await dbContext.Database.EnsureCreatedAsync(cancellationToken);
                dbContext.RunHistory.Add(entity);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation($"Job {JobName} result written to DB at time {DateTime.Now}");
        }
    }
}
