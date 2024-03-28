using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.ToTable("role");

        builder
            .Property(r => r.Id)
            .HasColumnName("id");

        builder
            .Property(r => r.DiscussionId)
            .HasColumnName("discussion_id");

        builder
            .Property(r => r.Name)
            .HasMaxLength(20)
            .HasColumnName("name");

        builder
            .Property(r => r.Permissions)
            .HasConversion(
                permissions => permissions.Select(p => p.Value),
                value => value.Select(v => Permission.Create(v).Value).ToList());

        builder
            .HasOne(r => r.DiscussionNavigation)
            .WithMany(d => d.RolesNavigation)
            .HasForeignKey(r => r.DiscussionId);
    }
}
