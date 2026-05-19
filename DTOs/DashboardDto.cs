namespace FactoryScheduler.Api.DTOs
{
    public class DashboardDto
    {
        public int TotalJobs { get; set; }
        public int TotalMachines { get; set; }
        public int RunningJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int EmergencyJobs { get; set; }
        public double AverageLoad { get; set; }
    }
}
