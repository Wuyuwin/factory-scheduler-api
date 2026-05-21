using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;

namespace FactoryScheduler.Api.Scheduling
{
    public interface ISchedulingStrategy
    {
        Task<Machine?> SelectMachineAsync(List<Machine> machines, AssignJobDto job);
    }
}
