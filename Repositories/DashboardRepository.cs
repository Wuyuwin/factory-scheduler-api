using FactoryScheduler.Api.Data;
using FactoryScheduler.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace FactoryScheduler.Api.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _db;
        public DashboardRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<int> GetTotalJobsAsync()
        {
            return await _db.MachineJobs.CountAsync();
        }
        public async Task<int> GetTotalMachinesAsync()
        {
            return await _db.Machines.CountAsync();
        }
        public async Task<int> GetCountByStatusAsync(JobStatus status)
        {
            return await _db.MachineJobs.CountAsync(mj => mj.Status == status);
        }
        public async Task<int> GetEmergencyJobsAsync()
        {
            return await _db.MachineJobs.CountAsync(mj => mj.Job.Priority == JobPriority.Emergency);
        }
        public async Task<double> GetAverageLoadAsync()
        {
            return await _db.Machines
                .Where(m => m.CurrentLoad > 0)
                .AverageAsync(m => (double)m.CurrentLoad / m.MaxLoad * 100);
        }
    }
}
