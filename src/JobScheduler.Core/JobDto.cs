using JobScheduler.Core.Enums;

namespace JobScheduler.Core
{
    public class JobDto
    {
        public string Name { get; set; } = string.Empty;
        public JobStatus Status { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}