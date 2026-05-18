using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.DTOs
{
    public class MachineJobDto
    {
        public int JobId { get; set; }
        public string JobName { get; set; } = string.Empty;
        public int Load { get; set; }
        public int JobStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public JobStatus Status { get; set; }

    }
}
