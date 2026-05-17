namespace FactoryScheduler.Api.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public string JobName { get; set; } = string.Empty;
        public int Load { get; set; }
        public int WorkMinutes { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public ICollection<MachineJob> MachineJobs { get; set; } 
            = new List<MachineJob>();
    }
}
