using FactoryScheduler.Api.Services;
using FactoryScheduler.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FactoryScheduler.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _service;

        public MachineController(IMachineService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<MachineDto>> GetAllMachines()
        {
            return await _service.GetAllAsync();
        }
        [HttpPost]
        public async Task<ActionResult<MachineDto>> Create(CreateMachineDto dto)
        {
            var res = await _service.CreateAsync(dto);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineDto>> GetMachineById(int id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<MachineDto>> UpdateMachine(int id, UpdateMachineDto dto)
        {
            var res = await _service.UpdateAsync(id, dto);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<MachineDto>> DeleteMachine(int id)
        {
            var res = await _service.DeleteAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpGet("available")]
        public async Task<ActionResult<List<MachineDto>>> GetAvailableMachines()
        {
            var res = await _service.GetAvailableAsync();
            return Ok(res);
        }
        [HttpGet("{id}/jobs")]
        public async Task<ActionResult<List<MachineJobDto>>> GetMachineJobs(int id)
        {
            var res = await _service.GetMachineJobAsync(id);
            return Ok(res);
        }
        [HttpGet("{id}/timeline")]
        public async Task<ActionResult<List<MachineTimelineDto>>> GetMachineTimeline(int id)
        {
            var res = await _service.GetMachineTimelineAsync(id);
            return Ok(res);
        }
    }
}
