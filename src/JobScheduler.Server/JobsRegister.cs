using JobScheduler.Server.Jobs;

namespace JobScheduler.Server
{
    public static class JobsRegister
    {
        public static void AddJobs(this IServiceCollection services)
        {
            services.AddHostedService<IntervalExampleJob>();
            services.AddHostedService<SpecificTimeExampleJob>();
        }
    }
}
