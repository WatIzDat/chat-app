using Domain.Discussions;
using Domain.Roles;
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

        //builder
        //    .Property(u => u.Discussions)
        //    .HasConversion(
        //        discussions => discussions.Value.ToList(),
        //        value => DiscussionsList.Create(value.ToList()).Value)
        //    .HasColumnName("discussions");

        builder
            .Ignore(u => u.Discussions);

        builder
            .Ignore(u => u.Roles);

        builder
            .HasMany(u => u.JoinedDiscussionsNavigation)
            .WithMany(d => d.JoinedUsersNavigation)
            .UsingEntity(
                "user_to_joined_discussion",
                l => l.HasOne(typeof(Discussion)).WithMany().HasForeignKey("discussion_id"),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("user_id"));

        builder
            .HasMany(u => u.RolesNavigation)
            .WithMany(r => r.UsersNavigation)
            .UsingEntity(
                "user_to_added_role",
                l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("role_id"),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("user_id"));

        //builder
        //    .Property(u => u.Roles)
        //    .HasConversion(
        //        roles => roles.Value.ToList(),
        //        value => RolesList.Create(value.ToList()).Value)
        //    .HasColumnName("roles");

        builder
            .Property(u => u.IsDeleted)
            .HasColumnName("is_deleted");

        builder
            .HasIndex(u => u.Username)
            .IsUnique()
            .HasFilter("is_deleted = FALSE");

        builder
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("is_deleted = FALSE");
    }
}
