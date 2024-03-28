using Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.ToTable("message");

        builder
            .Property(m => m.Id)
            .HasColumnName("id");

        builder
            .Property(m => m.UserId)
            .HasColumnName("user_id");

        builder
            .Property(m => m.DiscussionId)
            .HasColumnName("discussion_id");

        builder
            .Property(m => m.Contents)
            .HasMaxLength(500)
            .HasColumnName("contents");

        builder
            .Property(m => m.DateSentUtc)
            .HasColumnName("date_sent_utc");

        builder
            .Property(m => m.IsEdited)
            .HasColumnName("is_edited");

        builder
            .HasOne(m => m.UserNavigation)
            .WithMany(u => u.MessagesNavigation)
            .HasForeignKey(m => m.UserId);

        builder
            .HasOne(m => m.DiscussionNavigation)
            .WithMany(d => d.MessagesNavigation)
            .HasForeignKey(m => m.DiscussionId);
    }
}
