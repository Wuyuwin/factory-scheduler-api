using FactoryScheduler.Api.DTOs;

namespace FactoryScheduler.Api.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetSummaryAsync();
    }
}
