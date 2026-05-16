using FactoryScheduler.Api.Entities;    

namespace FactoryScheduler.Api.Repositories;
public interface IMachineRepository
{
    Task<List<Machine>> GetAllAsync();
    Task<Machine> AddAsync(Machine machine);
    Task<Machine?> GetByIdAsync(int id);
    Task<Machine?> UpdateAsync(int id, Machine machine);
    Task<Machine?> DeleteAsync(int id);
    Task<List<Machine>> GetAvailableAsync();
    Task SaveChangesAsync();
}
