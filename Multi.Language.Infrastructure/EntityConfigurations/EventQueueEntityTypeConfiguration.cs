using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multi.Language.Infrastructure.EventSourcing;

namespace Multi.Language.Infrastructure.EntityConfigurations
{
    public class EventQueueEntityTypeConfiguration : IEntityTypeConfiguration<EventQueue>
    {
        public void Configure(EntityTypeBuilder<EventQueue> builder)
        {
            builder.Ignore(x => x.DomainEvents);
            builder.ToTable("EventQueue", "es");
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.MessageType).HasColumnName("MessageType");
            builder.Property(x => x.Stream).HasColumnName("Stream");
            builder.Property(x => x.AggregateId).HasColumnName("AggregateId");
            builder.Property(x => x.TransactionId).HasColumnName("TransactionId");
            builder.Property(x => x.EventName).HasColumnName("EventName").IsRequired(); 
            builder.Property(x => x.EventDate).HasColumnName("EventDate").IsRequired();
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.IpAddress).HasColumnName("IpAddress");
            builder.Property(x => x.Data).HasColumnName("Data");
        }
    }
}
