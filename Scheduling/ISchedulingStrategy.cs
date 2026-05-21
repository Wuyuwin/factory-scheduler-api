using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;

namespace FactoryScheduler.Api.Scheduling
{
    public interface ISchedulingStrategy
    {
        string Mode { get; }
        string Message { get; }
        Task<Machine?> SelectMachineAsync(List<Machine> machines, AssignJobDto job);
    }
}
