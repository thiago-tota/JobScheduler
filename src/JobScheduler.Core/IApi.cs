namespace JobScheduler.Core
{
    public interface IApi
    {
        public Task<List<JobDto>> GetSchedulerHistory();
    }
}
