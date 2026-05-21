using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;
using FactoryScheduler.Api.Enums;
using FactoryScheduler.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryScheduler.Api.Controllers
{
    [ApiController]
    [Route("api/ai-test")]
    public class AiTestController : ControllerBase
    {
        private readonly OllamaSchedulerService _ollamaSchedulerService;
        public AiTestController(OllamaSchedulerService ollamaSchedulerService)
        {
            _ollamaSchedulerService = ollamaSchedulerService;
        }

        [HttpPost]
        public async Task<IActionResult> Test()
        {
            var machines = new List<Machine>
            {
                new () { Id = 1, Name = "Machine 1", MaxLoad = 100, CurrentLoad = 50, WorkMinutes = 30, IsRunning = true, Ratio = 1.0 },
                new () { Id = 2, Name = "Machine 2", MaxLoad = 150, CurrentLoad = 20, WorkMinutes = 10, IsRunning = false, Ratio = 1.5 },
                new () { Id = 3, Name = "Machine 3", MaxLoad = 200, CurrentLoad = 80, WorkMinutes = 60, IsRunning = true, Ratio = 0.8 }
            };
            var dto = new AssignJobDto
            {
                JobName = "Test Job",
                Load = 40,
                WorkMinutes = 20,
                Priority = JobPriority.High
            };
            var result = await _ollamaSchedulerService.SelectMachineAsync(machines, dto);
            return Ok(result);
        }
    }
}
