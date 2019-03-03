using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLazyLoadingProxies();

        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}
