using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Repositories;
using FactoryScheduler.Api.Entities;    

namespace FactoryScheduler.Api.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;

        public MachineService(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<List<MachineDto>> GetAllAsync()
        {
            var machines = await _machineRepository.GetAllAsync();
            return machines.Select(m => new MachineDto
            {
                Id = m.Id,
                Name = m.Name,
                MaxLoad = m.MaxLoad,
                CurrentLoad = m.CurrentLoad,
                WorkMinutes = m.WorkMinutes,
                IsRunning = m.IsRunning
            }).ToList();
        }

        public async Task<MachineDto> CreateAsync(CreateMachineDto dto)
        {
            var machine = new Machine
            {
                Name = dto.Name,
                MaxLoad = dto.MaxLoad,
                CurrentLoad = dto.CurrentLoad,
                WorkMinutes = dto.WorkMinutes,
                IsRunning = dto.IsRunning
            };
            var addedMachine = await _machineRepository.AddAsync(machine);
            return new MachineDto
            {
                Id = addedMachine.Id,
                Name = addedMachine.Name,
                MaxLoad = addedMachine.MaxLoad,
                CurrentLoad = addedMachine.CurrentLoad,
                WorkMinutes = addedMachine.WorkMinutes,
                IsRunning = addedMachine.IsRunning
            };
        }
        public async Task<MachineDto?> GetByIdAsync(int id)
        {
            var machine = await _machineRepository.GetByIdAsync(id);
            if (machine == null) return null;
            return new MachineDto
            {
                Id = machine.Id,
                Name = machine.Name,
                MaxLoad = machine.MaxLoad,
                CurrentLoad = machine.CurrentLoad,
                WorkMinutes = machine.WorkMinutes,
                IsRunning = machine.IsRunning
            };
        }
        public async Task<MachineDto?> UpdateAsync(int id, UpdateMachineDto dto)
        {
            var machine = new Machine
            {
                Name = dto.Name,
                MaxLoad = dto.MaxLoad,
                CurrentLoad = dto.CurrentLoad,
                WorkMinutes = dto.WorkMinutes,
                IsRunning = dto.IsRunning
            };
            var updatedMachine = await _machineRepository.UpdateAsync(id, machine);
            if (updatedMachine == null) return null;
            return new MachineDto
            {
                Id = updatedMachine.Id,
                Name = updatedMachine.Name,
                MaxLoad = updatedMachine.MaxLoad,
                CurrentLoad = updatedMachine.CurrentLoad,
                WorkMinutes = updatedMachine.WorkMinutes,
                IsRunning = updatedMachine.IsRunning
            };
        }
        public async Task<MachineDto?> DeleteAsync(int id)
        {
            var machine = await _machineRepository.DeleteAsync(id);
            if (machine == null) return null;
            return new MachineDto
            {
                Id = machine.Id,
                Name = machine.Name,
                MaxLoad = machine.MaxLoad,
                CurrentLoad = machine.CurrentLoad,
                WorkMinutes = machine.WorkMinutes,
                IsRunning = machine.IsRunning
            };
        }
        public async Task<List<MachineDto>> GetAvailableAsync()
        {
            var machines = await _machineRepository.GetAvailableAsync();
            return machines.Select(m => new MachineDto
            {
                Id = m.Id,
                Name = m.Name,
                MaxLoad = m.MaxLoad,
                CurrentLoad = m.CurrentLoad,
                WorkMinutes = m.WorkMinutes,
                IsRunning = m.IsRunning
            }).ToList();
        }
    }
}
