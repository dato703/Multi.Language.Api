using System;
using Microsoft.EntityFrameworkCore;
using Multi.Language.Domain.AggregatesModel.UserAggregate;

namespace Multi.Language.Infrastructure.EntityConfigurations
{
    public static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, IEntityTypeConfiguration<TEntity> entityConfiguration) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            var user = new User(Guid.NewGuid(), "Dato", "1234567", "dato703@gmail.com", UserRole.SuperAdministrator);
            modelBuilder.Entity<User>().HasData(user);
        }
    }
}
