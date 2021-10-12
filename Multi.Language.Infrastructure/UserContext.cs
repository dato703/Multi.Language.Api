using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Multi.Language.Domain.AggregatesModel.UserAggregate;
using Multi.Language.Infrastructure.EntityConfigurations;
using Multi.Language.Infrastructure.EventSourcing;

namespace Multi.Language.Infrastructure
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EventQueue> EventQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Seed();
        }
    }
}
