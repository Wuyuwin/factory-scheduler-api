using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalJobsAsync();
        Task<int> GetTotalMachinesAsync();
        Task<int> GetCountByStatusAsync(JobStatus status);
        Task<int> GetEmergencyJobsAsync();
        Task<double> GetAverageLoadAsync();
    }
}
