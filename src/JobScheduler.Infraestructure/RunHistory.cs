using JobScheduler.Core.Enums;

namespace JobScheduler.Infraestructure.Data
{
    public class RunHistory
    {
        public Guid Id { get; set; }
        public string JobName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public JobStatus Status { get; set; }
    }
}