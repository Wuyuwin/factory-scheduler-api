using FactoryScheduler.Api.DTOs;

namespace FactoryScheduler.Api.Services
{
    public interface IScheduleService
    {
        Task<ScheduleResultDto?> AssignJobAsync(AssignJobDto dto);
    }
}
