namespace JobScheduler.Core
{
    public class JobSchedulesDto
    {
        public const string JobSchedules = "JobSchedules";

        public List<JobDto> Jobs { get; }

        public JobSchedulesDto()
        {
            Jobs = new List<JobDto>();
        }
    }
}
