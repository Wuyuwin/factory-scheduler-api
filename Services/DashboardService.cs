using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Repositories;
using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDto> GetSummaryAsync()
        {
            var TotalJobs = await _dashboardRepository.GetTotalJobsAsync();
            var totalMachines = await _dashboardRepository.GetTotalMachinesAsync();
            var RunningJobs = await _dashboardRepository.GetCountByStatusAsync(JobStatus.Running);
            var CompletedJobs = await _dashboardRepository.GetCountByStatusAsync(JobStatus.Completed);
            var EmergencyJobs = await _dashboardRepository.GetEmergencyJobsAsync();
            var averageLoad = await _dashboardRepository.GetAverageLoadAsync();

            return new DashboardDto
            {
                TotalMachines = totalMachines,
                TotalJobs = TotalJobs,
                RunningJobs = RunningJobs,
                CompletedJobs = CompletedJobs,
                EmergencyJobs = EmergencyJobs,
                AverageLoad = averageLoad
            };
        }
    }
}
