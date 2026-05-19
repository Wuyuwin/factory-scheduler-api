using FactoryScheduler.Api.Data;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;   
using Microsoft.EntityFrameworkCore;

namespace FactoryScheduler.Api.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _db;
        public JobRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<MachineJob>> GetAllJobsAsync()
        {
            return await _db.MachineJobs
                .Include(mj => mj.Job)
                .Include(mj => mj.Machine)
                .OrderByDescending(mj => mj.StartTime)
                .ToListAsync();
        }
        public async Task<MachineJob?> GetByIdAsync(int id)
        {
            return await _db.MachineJobs
                .Include(mj => mj.Job)
                .Include(mj => mj.Machine)
                .FirstOrDefaultAsync(mj => mj.Id == id);
        }
        public async Task<List<MachineJob>> GetJobsByStatusAsync(JobStatus jobStatus)
        {
            return await _db.MachineJobs
                .Include(mj => mj.Job)
                .Include(mj => mj.Machine)
                .Where(mj => mj.Status == jobStatus)
                .ToListAsync();
        }
        public async Task<List<MachineJob>> GetJobsByPriorityAsync(JobPriority priority)
        {
            return await _db.MachineJobs
                .Include(mj => mj.Job)
                .Include(mj => mj.Machine)
                .Where(mj => mj.Job.Priority == priority)
                .OrderByDescending(mj => mj.StartTime)
                .ToListAsync();
        }
    }
}
