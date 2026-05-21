using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Repositories;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;
using FactoryScheduler.Api.Scheduling;

namespace FactoryScheduler.Api.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly ISchedulingStrategy _schedulingStrategy;
        public ScheduleService(IMachineRepository machineRepository, ISchedulingStrategy schedulingStrategy)
        {
            _machineRepository = machineRepository;
            _schedulingStrategy = schedulingStrategy;
        }
        public async Task<ScheduleResultDto?> AssignJobAsync(AssignJobDto dto)
        {
            if (dto.Priority == JobPriority.Emergency)
            { return await AssignEmergencyJobAsync(dto); }
            var machines = await _machineRepository.GetAvailableJobAsync(dto.Load);
            var now = DateTime.UtcNow;
            var suitableMachine = await _schedulingStrategy.SelectMachineAsync(machines, dto);
            if (suitableMachine == null) { return null; }
            var schedule = await _machineRepository.GetMachineTimelineAsync(suitableMachine.Id);

            var lastUnfinishedJob = schedule
                .Where(x => x.Status != JobStatus.Completed)
                .OrderByDescending(x => x.EndTime)
                .FirstOrDefault();

            var startTime = lastUnfinishedJob == null
                ? now
                : lastUnfinishedJob.EndTime;

            var endTime = startTime.AddMinutes(dto.WorkMinutes / suitableMachine.Ratio);

            var status = lastUnfinishedJob == null
                ? JobStatus.Running
                : JobStatus.Pending;
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
                Status = status,
                Priority = dto.Priority
            };
            await _machineRepository.AddMachineJobAsync(machineJob);
            suitableMachine.CurrentLoad += dto.Load;
            var addedWorkMinutes = (int)Math.Ceiling(dto.WorkMinutes / suitableMachine.Ratio);
            suitableMachine.WorkMinutes += addedWorkMinutes;
            await _machineRepository.SaveChangesAsync();
            return new ScheduleResultDto
            {
                JobName = createJob.JobName,
                MachineId = suitableMachine.Id,
                MachineName = suitableMachine.Name,
                UpdatedLoad = suitableMachine.CurrentLoad,
                WorkMinutes = suitableMachine.WorkMinutes,
                Priority = createJob.Priority.ToString(),
                Message = 
                    $"{_schedulingStrategy.Mode} Mode:" +
                    $" Job assigned to machine {suitableMachine.Name},{suitableMachine.Id}." +
                    $"Reason: {_schedulingStrategy.Message}"
            };
        }
        private async Task<ScheduleResultDto?> AssignEmergencyJobAsync(AssignJobDto dto)
        {
            var machines = await _machineRepository.GetAvailableJobAsync(dto.Load);
            if (machines.Count == 0) { return null; }
            var now = DateTime.UtcNow;
            var suitableMachine = await _schedulingStrategy.SelectMachineAsync(machines, dto);
            if (suitableMachine == null) { return null; }
            var schudule = await _machineRepository.GetMachineTimelineAsync(suitableMachine.Id);
            var FirstPendingJob = schudule
                    .Where(s => s.Status == JobStatus.Pending)
                    .OrderBy(s => s.StartTime)
                    .FirstOrDefault();
            var runningJob = schudule
                    .Where(s => s.Status == JobStatus.Running)
                    .OrderByDescending(s => s.EndTime)
                    .FirstOrDefault();
            var startTime = runningJob != null && runningJob.EndTime > now
                ? runningJob.EndTime
                : (FirstPendingJob?.StartTime ?? now);
            if (startTime < now) {startTime = now;}
            var duration = dto.WorkMinutes / suitableMachine.Ratio;
            var endTime = startTime.AddMinutes(duration);
            foreach (var j in schudule.Where(s => 
                s.Status == JobStatus.Pending && s.StartTime >= startTime))
            {
                j.StartTime = j.StartTime.AddMinutes(duration);
                j.EndTime = j.EndTime.AddMinutes(duration);
            }
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
                Priority = dto.Priority,
                Status = runningJob != null
                    ? JobStatus.Pending
                    : JobStatus.Running
            };
            await _machineRepository.AddMachineJobAsync(machineJob);
            suitableMachine.CurrentLoad += dto.Load;
            var addedWorkMinutes = (int)Math.Ceiling(dto.WorkMinutes / suitableMachine.Ratio);
            suitableMachine.WorkMinutes += addedWorkMinutes;
            await _machineRepository.SaveChangesAsync();
            return new ScheduleResultDto
            {
                JobName = createJob.JobName,
                MachineId = suitableMachine.Id,
                MachineName = suitableMachine.Name,
                UpdatedLoad = suitableMachine.CurrentLoad,
                WorkMinutes = suitableMachine.WorkMinutes,
                Priority = createJob.Priority.ToString(),
                Message = 
                    $"{_schedulingStrategy.Mode} Mode:" +
                    $" Job assigned to machine {suitableMachine.Name},{suitableMachine.Id}." +
                    $"Reason: {_schedulingStrategy.Message}"
            };
        }
    }
}
