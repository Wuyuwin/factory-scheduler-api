using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryScheduler.Api.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;

        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }
        [HttpPost("assign-job")]
        public async Task<ActionResult<ScheduleResultDto>> AssignJob(AssignJobDto dto)
        {
            var res = await _service.AssignJobAsync(dto);
            if (res == null) return NotFound(new { Message = "No suitable machine found for the job." });
            return Ok(res);
        }
    }
}
