using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Enums;
using FactoryScheduler.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryScheduler.Api.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetJobsByStatus(string status)
        {
            if (!Enum.TryParse<JobStatus>(status, true, out var jobStatus))
            {
                return BadRequest(new { Message = "Invalid job status." });
            }
            var jobs = await _jobService.GetJobsByStatusAsync(jobStatus);
            return Ok(jobs);
        }
    }
}