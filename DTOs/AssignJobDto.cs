namespace FactoryScheduler.Api.DTOs
{
    public class AssignJobDto
    {
        public string JobName { get; set; } = string.Empty;
        public int Load { get; set; }
        public int WorkMinutes { get; set; }
    }
}
