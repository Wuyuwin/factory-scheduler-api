using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Repositories;

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
            var machines = await _machineRepository.GetAvailableAsync();
            var suitableMachine = machines.FirstOrDefault();
            if (suitableMachine == null) { return null; }
            suitableMachine.CurrentLoad += dto.Load;
            suitableMachine.WorkMinutes += dto.WorkMinutes;
            await _machineRepository.SaveChangesAsync();
            return new ScheduleResultDto
            {
                JobName = dto.JobName,
                MachineId = suitableMachine.Id,
                MachineName = suitableMachine.Name,
                UpdatedLoad = suitableMachine.CurrentLoad,
                WorkMinutes = suitableMachine.WorkMinutes
            };
        }
    }
}
