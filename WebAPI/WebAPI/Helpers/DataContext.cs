using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAPI.Entities;

namespace WebAPI.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLazyLoadingProxies();

        public DbSet<User> Users { get; set; }
        public DbSet<UserToRegister> UsersToCompleteRegistration { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<PendingAuth> PendingAuths { get; set; }
        public DbSet<FirebaseToken> FirebaseTokens { get; set; }
    }
}
