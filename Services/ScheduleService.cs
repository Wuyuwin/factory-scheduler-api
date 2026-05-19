using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Repositories;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IMachineRepository _machineRepository;
        public ScheduleService(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }
        public async Task<ScheduleResultDto?> AssignJobAsync(AssignJobDto dto)
        {
            var machines = await _machineRepository.GetAvailableJobAsync(dto.Load);
            var now = DateTime.UtcNow;
            var suitableMachine =
                dto.Priority == JobPriority.Emergency ||
                dto.Priority == JobPriority.High
                ? machines
                    .OrderBy(m =>(m.WorkMinutes / m.Ratio) + (dto.WorkMinutes / m.Ratio))
                    .ThenBy(m => (double)m.CurrentLoad / m.MaxLoad)
                    .First()
                : machines 
                    .OrderBy(m => (m.WorkMinutes / m.Ratio) + (dto.WorkMinutes / m.Ratio))
                    .ThenBy(m => (double)(m.CurrentLoad + dto.Load) / m.MaxLoad)
                    .First();
            if (suitableMachine == null) { return null; }
            var startTime = now.AddMinutes(suitableMachine.WorkMinutes / suitableMachine.Ratio);
            var endTime = startTime.AddMinutes(dto.WorkMinutes / suitableMachine.Ratio);
            var hasRunningJob = await _machineRepository.HasRunningJobAsync(suitableMachine.Id);
            var status = hasRunningJob ? JobStatus.Pending : JobStatus.Running;
            var job = new Job
            {
                JobName = dto.JobName,
                Load = dto.Load,
                WorkMinutes = dto.WorkMinutes,
                Priority = dto.Priority
            };
            var createJob = await _machineRepository.AddJobAsync(job);
            var machineJob = new MachineJob
            {
                MachineId = suitableMachine.Id,
                JobId = createJob.Id,
                StartTime = startTime,
                EndTime = endTime,
                Status = status
            };
            await _machineRepository.AddMachineJobAsync(machineJob);
            suitableMachine.CurrentLoad += dto.Load;
            suitableMachine.WorkMinutes += dto.WorkMinutes;
            await _machineRepository.SaveChangesAsync();
            return new ScheduleResultDto
            {
                JobName = createJob.JobName,
                MachineId = suitableMachine.Id,
                MachineName = suitableMachine.Name,
                UpdatedLoad = suitableMachine.CurrentLoad,
                WorkMinutes = suitableMachine.WorkMinutes,
                Priority = createJob.Priority.ToString(),
                Message = $"Job assigned to machine {suitableMachine.Name},{suitableMachine.Id}"
            };
        }
    }
}
