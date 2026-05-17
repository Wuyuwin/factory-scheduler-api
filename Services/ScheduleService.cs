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
            var suitableMachine = machines
                .OrderBy(m =>
                    now
                    .AddMinutes(m.WorkMinutes / m.Ratio)
                    .AddMinutes(dto.WorkMinutes / m.Ratio))
                .First();
            if (suitableMachine == null) { return null; }
            var startTime = now.AddMinutes(suitableMachine.WorkMinutes / suitableMachine.Ratio);
            var endTime = startTime.AddMinutes(dto.WorkMinutes / suitableMachine.Ratio);
            var status = startTime <= now ? JobStatus.Running : JobStatus.Pending;
            var job = new Job
            {
                JobName = dto.JobName,
                Load = dto.Load,
                WorkMinutes = dto.WorkMinutes
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
                Message = $"Job assigned to machine {suitableMachine.Name},{suitableMachine.Id}"
            };
        }
    }
}
