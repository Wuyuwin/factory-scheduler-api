using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Services
{
    public interface IJobService
    {
        Task<List<JobDto>> GetAllJobsAsync();
        Task<JobDto?> GetByIdAsync(int id);
        Task<List<JobDto>> GetJobsByStatusAsync(JobStatus jobStatus);
        Task<List<JobDto>> GetJobsByPriorityAsync(JobPriority priority);
    }
}
