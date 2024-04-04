using Domain.Bans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal class BanEntityConfiguration : IEntityTypeConfiguration<Ban>
{
    public void Configure(EntityTypeBuilder<Ban> builder)
    {
        builder.HasKey(b => b.Id);

        builder.ToTable("ban");

        builder
            .Property(b => b.Id)
            .HasColumnName("id");

        builder
            .Property(b => b.UserId)
            .HasColumnName("user_id");

        builder
            .Property(b => b.DiscussionId)
            .HasColumnName("discussion_id");

        builder
            .Property(b => b.DateOfBanUtc)
            .HasColumnName("date_of_ban_utc");

        builder.ComplexProperty(b => b.BanDetails, banDetailsBuilder =>
        {
            banDetailsBuilder
                .Property(b => b.IsBanPermanent)
                .HasColumnName("is_ban_permanent");

            banDetailsBuilder
                .Property(b => b.DateWillBeUnbannedUtc)
                .HasColumnName("date_will_be_unbanned_utc");
        });

        builder
            .Property(b => b.IsUnbanned)
            .HasColumnName("is_unbanned");

        builder
            .HasOne(b => b.UserNavigation)
            .WithMany(u => u.BansNavigation)
            .HasForeignKey(b => b.UserId);

        builder
            .HasOne(b => b.DiscussionNavigation)
            .WithMany(d => d.BansNavigation)
            .HasForeignKey(b => b.DiscussionId);
    }
}
