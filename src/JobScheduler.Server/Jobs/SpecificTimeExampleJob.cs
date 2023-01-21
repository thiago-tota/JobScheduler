namespace JobScheduler.Server.Jobs
{
    public class SpecificTimeExampleJob : JobBase
    {
        private readonly ILogger<SpecificTimeExampleJob> _logger;

        protected override string JobName => nameof(SpecificTimeExampleJob);

        public SpecificTimeExampleJob(ILogger<SpecificTimeExampleJob> logger, IServiceProvider serviceProvider) 
            : base(logger, serviceProvider, TimeOnly.FromDateTime(DateTime.Now.AddSeconds(15)))
        {
            _logger = logger;
        }

        protected override Task JobAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"TODO: Here the job should do something not just log.");
            return Task.CompletedTask;
        }
    }
}
