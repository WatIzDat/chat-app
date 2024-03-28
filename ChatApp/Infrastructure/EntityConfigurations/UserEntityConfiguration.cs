using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.ToTable("user");

        builder
            .Property(u => u.Id)
            .HasColumnName("id");

        builder
            .Property(u => u.Username)
            .HasMaxLength(20)
            .HasColumnName("username");

        builder
            .Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value)
            .HasColumnName("email");

        builder
            .Property(u => u.DateCreatedUtc)
            .HasColumnName("date_created_utc");

        builder
            .Property(u => u.AboutSection)
            .HasConversion(
                aboutSection => aboutSection.Value,
                value => AboutSection.Create(value).Value)
            .HasColumnName("about_section");

        builder
            .Property(u => u.Discussions)
            .HasConversion(
                discussions => discussions.Value,
                value => DiscussionsList.Create(value.ToList()).Value);

        builder
            .HasMany(u => u.DiscussionsNavigation)
            .WithOne();

        builder
            .Property(u => u.Roles)
            .HasConversion(
                roles => roles.Value,
                value => RolesList.Create(value.ToList()).Value);

        builder
            .Property(u => u.IsDeleted)
            .HasColumnName("is_deleted");

        builder
            .HasIndex(u => u.Username)
            .IsUnique()
            .HasFilter("is_deleted = FALSE");
    }
}
