using FactoryScheduler.Api.DTOs;

namespace FactoryScheduler.Api.Services
{
    public interface IMachineService
    {
        Task<List<MachineDto>> GetAllAsync();

        Task<MachineDto> CreateAsync(CreateMachineDto dto);
        Task<MachineDto?> GetByIdAsync(int id);

        Task<MachineDto?> UpdateAsync(int id, UpdateMachineDto dto);
        Task<MachineDto?> DeleteAsync(int id); 
        Task<List<MachineDto>> GetAvailableAsync();
        Task<List<MachineJobDto>> GetMachineJobAsync(int machineId);
        Task<List<MachineTimelineDto>> GetMachineTimelineAsync(int machineId);
    }
}
