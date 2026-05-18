namespace FactoryScheduler.Api.DTOs
{
    public class MachineTimelineDto
    {
        public int JobId { get; set; }
        public string JobName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Load { get; set; }
        public int MachineWork { get; set; }
    }
}
