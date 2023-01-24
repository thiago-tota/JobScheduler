namespace JobScheduler.Server.Jobs
{
    public class IntervalExampleJob : JobBase
    {
        private readonly ILogger<IntervalExampleJob> _logger;

        private const int _intervalInSeconds = 20;

        protected override string JobName => nameof(IntervalExampleJob);

        public IntervalExampleJob(ILogger<IntervalExampleJob> logger, IServiceProvider serviceProvider) 
            : base(logger, serviceProvider, _intervalInSeconds)
        {
            _logger = logger;
        }

        protected override Task JobAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"TODO: Body of job.");
            return Task.CompletedTask;
        }
    }
}
