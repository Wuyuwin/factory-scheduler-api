using FactoryScheduler.Api.Data;
using FactoryScheduler.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace FactoryScheduler.Api.BackgroundServices
{
    public class JobStatusUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public JobStatusUpdaterService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var now = DateTime.UtcNow;
                    var machineJobs = await dbContext.MachineJobs
                        .Include(mj => mj.Job)
                        .Where(mj => mj.Status != JobStatus.Completed)
                        .ToListAsync(stoppingToken);
                    foreach (var machineJob in machineJobs)
                    {
                        if (machineJob.EndTime <= now)
                        {
                            machineJob.Status = JobStatus.Completed;
                        }
                        else if (machineJob.StartTime <= now)
                        {
                            machineJob.Status = JobStatus.Running;
                        }
                        var machine = await dbContext.Machines
                            .FirstOrDefaultAsync(m => m.Id == machineJob.MachineId, stoppingToken);
                        if (machine != null && machineJob.Job is not null)
                        {
                            machine.CurrentLoad -= machineJob.Job.Load;
                            machine.WorkMinutes -= machineJob.Job.WorkMinutes;
                            if (machine.CurrentLoad <= 0)
                            {
                                machine.CurrentLoad = 0;
                            }
                            if (machine.WorkMinutes <= 0)
                            {
                                machine.WorkMinutes = 0;
                            }
                        }
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
        }
    }
}
