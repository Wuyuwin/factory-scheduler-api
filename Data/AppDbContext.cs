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
    }
}
