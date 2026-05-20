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
            var machines = await _db.Machines
                .Where(x => x.MaxLoad > 0)
                .ToListAsync();

            if (machines.Count == 0)
                return 0;

            return machines.Average(x =>
                (double)x.CurrentLoad / x.MaxLoad * 100);
        }
    }
}
