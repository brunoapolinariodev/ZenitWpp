using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Content).IsRequired().HasMaxLength(4096);
        builder.Property(x => x.Type).HasConversion<string>().IsRequired();
        builder.Property(x => x.Direction).HasConversion<string>().IsRequired();
        builder.Property(x => x.SentAt).IsRequired();
        builder.Property(x => x.ConversationId).IsRequired();
    }
}
