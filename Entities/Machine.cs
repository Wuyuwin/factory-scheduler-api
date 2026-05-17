namespace FactoryScheduler.Api.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public int MaxLoad { get; set; }
        public int CurrentLoad { get; set; }
        public int WorkMinutes { get; set; }
        public bool IsRunning { get; set; }
        public double Ratio { get; set; } = 1.0;
        public ICollection<MachineJob> MachineJobs { get; set; }
            = new List<MachineJob>();
    }
}
