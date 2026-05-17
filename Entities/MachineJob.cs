using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Entities
{
    public class MachineJob
    {
        public int Id { get; set; }
        public int MachineId { get; set; }

        public Machine Machine { get; set; } = null!;
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public JobStatus Status { get; set; }
    }
}
