using Microsoft.EntityFrameworkCore;
using Multi.Language.Domain.UserAggregate;
using Multi.Language.Infrastructure.EntityConfigurations;

namespace Multi.Language.Infrastructure
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}
