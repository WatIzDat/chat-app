using Domain.Discussions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal class DiscussionEntityConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.HasKey(d => d.Id);

        builder.ToTable("discussion");

        builder
            .Property(d => d.Id)
            .HasColumnName("id");

        builder
            .Property(d => d.UserCreatedBy)
            .HasColumnName("user_created_by");

        builder
            .Property(d => d.Name)
            .HasColumnName("name");

        builder
            .Property(d => d.DateCreatedUtc)
            .HasColumnName("date_created_utc");

        builder
            .Property(d => d.IsDeleted)
            .HasColumnName("is_deleted");

        builder
            .HasOne(d => d.User)
            .WithMany(u => u.DiscussionsNavigation)
            .HasForeignKey(d => d.UserCreatedBy);
    }
}
