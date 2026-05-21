using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;

namespace FactoryScheduler.Api.Scheduling
{
    public class EarliestFinishTimeStrategy : ISchedulingStrategy
    {
        public string Mode => "Rule";
        public string Message => "Selected by Earliest Finish Time";
        public Task<Machine?> SelectMachineAsync(List<Machine> machines, AssignJobDto dto)
        {
            var priorityWeight = GetPriorityWeight(dto.Priority);
            var machine = machines
                .OrderBy(m =>
                        (m.WorkMinutes / m.Ratio)
                        + ((dto.WorkMinutes *priorityWeight) / m.Ratio))
                .ThenBy(m =>
                        (double)(m.CurrentLoad + dto.Load) / m.MaxLoad)
                .FirstOrDefault();
            return Task.FromResult(machine);
        }
        private static double GetPriorityWeight(JobPriority priority)
        {
            return priority switch
            {
                JobPriority.Low => 1.5,
                JobPriority.Normal => 1.0,
                JobPriority.High => 0.8,
                JobPriority.Emergency => 0.5,
                _ => 1.0
            };
        }
    }
}
