using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multi.Language.Domain.AggregatesModel.UserAggregate;

namespace Multi.Language.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(x => x.DomainEvents);
            builder.ToTable("Users", "core");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).HasColumnName("UserName");
            builder.Property(x => x.Password).HasColumnName("Password");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(t => t.UserRole).HasColumnName("UserRole");
        }
    }
}
