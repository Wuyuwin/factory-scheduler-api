using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Repositories
{
    public interface IJobRepository
    {
        Task<List<MachineJob>> GetAllJobsAsync();
        Task<MachineJob?> GetByIdAsync(int id);
        Task<List<MachineJob>> GetJobsByStatusAsync(JobStatus jobStatus);
        Task<List<MachineJob>> GetJobsByPriorityAsync(JobPriority priority);
    }
}
