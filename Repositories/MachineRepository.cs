using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FactoryScheduler.Api.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly AppDbContext _db;
    public MachineRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Machine>> GetAllAsync()
    {
        return await _db.Machines.ToListAsync();
    }

    public async Task<Machine> AddAsync(Machine machine)
    {
        _db.Machines.Add(machine);
        await _db.SaveChangesAsync();
        return machine;
    }
    public async Task<Machine?> GetByIdAsync(int id)
    {
        return await _db.Machines.FindAsync(id);
    }
    public async Task<Machine?> UpdateAsync(int id, Machine machine)
    {
        var existingMachine = await _db.Machines.FindAsync(id);
        if (existingMachine == null) return null;
        existingMachine.Name = machine.Name;
        existingMachine.MaxLoad = machine.MaxLoad;
        existingMachine.CurrentLoad = machine.CurrentLoad;
        existingMachine.WorkMinutes = machine.WorkMinutes;
        existingMachine.IsRunning = machine.IsRunning;
        await _db.SaveChangesAsync();
        return existingMachine;
    }
    public async Task<Machine?> DeleteAsync(int id)
    {
        var machine = await _db.Machines.FindAsync(id);
        if (machine == null) return null;
        _db.Machines.Remove(machine);
        await _db.SaveChangesAsync();
        return machine;
    }
    public async Task<List<Machine>> GetAvailableAsync()
    {
        return await _db.Machines
            .Where(m => 
                   m.IsRunning && 
                   m.CurrentLoad <= m.MaxLoad)
            .OrderBy(m => (double)m.CurrentLoad / m.MaxLoad)
            .ToListAsync();
    }
    public async Task<List<Machine>> GetAvailableJobAsync(int Load)
    {
        return await _db.Machines
            .Where(m =>
                   m.IsRunning &&
                   m.CurrentLoad + Load <= m.MaxLoad)
            .ToListAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();   
    }
    public async Task<Job> AddJobAsync(Job job)
    {
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync();
        return job;
    }
    public async Task<MachineJob> AddMachineJobAsync(MachineJob machineJob)
    {
        _db.MachineJobs.Add(machineJob);
        await _db.SaveChangesAsync();
        return machineJob;
    }
    public async Task<List<MachineJob>> GetMachineJobAsync(int machineId)
    {
        return await _db.MachineJobs
            .Include(mj => mj.Job)
            .Where(mj => mj.MachineId == machineId)
            .ToListAsync();
    }
}