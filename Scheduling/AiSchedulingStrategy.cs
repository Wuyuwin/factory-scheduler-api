using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Services;

namespace FactoryScheduler.Api.Scheduling
{
    public class AiSchedulingStrategy : ISchedulingStrategy
    {
        public string Mode => "AI";
        public string Message { get; private set; } = string.Empty;
        private readonly OllamaSchedulerService _ollamaSchedulerService;
        public AiSchedulingStrategy(OllamaSchedulerService ollamaSchedulerService)
        {
            _ollamaSchedulerService = ollamaSchedulerService;
        }
        public async Task<Machine?> SelectMachineAsync(List<Machine> machines, AssignJobDto dto)
        {
            var aiResult = await _ollamaSchedulerService.SelectMachineAsync(machines, dto);
            Message = aiResult?.Message ?? "AI did not return a message.";
            var machine = machines.FirstOrDefault(m => m.Id == aiResult?.MachineId);
            if (machine == null)
            {
                machine = machines
                    .OrderBy(m =>
                        (m.WorkMinutes / m.Ratio)
                        +
                        (dto.WorkMinutes / m.Ratio))
                    .FirstOrDefault();
                Message = " AI fallback to default strategy.";
            }
                return machine;
        }
    }
}
