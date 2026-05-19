using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.DTOs
{
    public class JobDto
    {
        public int Id { get; set; }
        public string JobName { get; set; } = string.Empty;
        public int Load { get; set; }
        public int WorkMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string  Priority { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
