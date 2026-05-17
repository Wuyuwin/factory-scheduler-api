using Microsoft.EntityFrameworkCore;
using FactoryScheduler.Api.Entities;

namespace FactoryScheduler.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Machine> Machines => Set<Machine>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<MachineJob> MachineJobs => Set<MachineJob>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MachineJob>()
                .HasOne(mj => mj.Machine)
                .WithMany(m => m.MachineJobs)
                .HasForeignKey(mj => mj.MachineId);
            modelBuilder.Entity<MachineJob>()
                .HasOne(mj => mj.Job)
                .WithMany(j => j.MachineJobs)
                .HasForeignKey(mj => mj.JobId);
        }
    }
}
