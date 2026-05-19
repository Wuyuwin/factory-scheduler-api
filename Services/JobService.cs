using   FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;
using FactoryScheduler.Api.Repositories;

namespace FactoryScheduler.Api.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<List<JobDto>> GetAllJobsAsync()
        {
            var machineJobs = await _jobRepository.GetAllJobsAsync();
            return machineJobs.Select(MapToJobDto).ToList();
        }
        public async Task<JobDto?> GetByIdAsync(int id)
        {
            var machineJob = await _jobRepository.GetByIdAsync(id);
            if (machineJob == null) return null;
            return MapToJobDto(machineJob);
        }
        public async Task<List<JobDto>> GetJobsByStatusAsync(JobStatus jobStatus)
        {
            var machineJobs = await _jobRepository.GetJobsByStatusAsync(jobStatus);
            return machineJobs.Select(MapToJobDto).ToList();
        }
        private JobDto MapToJobDto(MachineJob machineJob)
        {
            return new JobDto
            {
                Id = machineJob.Id,
                JobName = machineJob.Job.JobName,
                Load = machineJob.Job.Load,
                WorkMinutes = machineJob.Job.WorkMinutes,
                Status = machineJob.Status.ToString(),
                MachineId = machineJob.MachineId,
                MachineName = machineJob.Machine.Name,
                StartTime = machineJob.StartTime,
                EndTime = machineJob.EndTime
            };
        }
    }
}
