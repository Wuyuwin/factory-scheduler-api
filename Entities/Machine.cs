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
    }
}
