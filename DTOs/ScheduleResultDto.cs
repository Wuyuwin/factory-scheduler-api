namespace FactoryScheduler.Api.DTOs
{
    public class ScheduleResultDto
    {
        public string JobName { get; set; } = string.Empty;
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public int UpdatedLoad { get; set; }
        public int WorkMinutes { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
