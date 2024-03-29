﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240329013948_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Bans.Ban", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("DateOfBanUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_ban_utc");

                    b.Property<Guid>("DiscussionId")
                        .HasColumnType("uuid")
                        .HasColumnName("discussion_id");

                    b.Property<bool>("IsUnbanned")
                        .HasColumnType("boolean")
                        .HasColumnName("is_unbanned");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.ComplexProperty<Dictionary<string, object>>("BanDetails", "Domain.Bans.Ban.BanDetails#BanDetails", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateTimeOffset?>("DateOfUnbanUtc")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("date_of_unban_utc");

                            b1.Property<bool>("IsBanPermanent")
                                .HasColumnType("boolean")
                                .HasColumnName("is_ban_permanent");
                        });

                    b.HasKey("Id");

                    b.HasIndex("DiscussionId");

                    b.HasIndex("UserId");

                    b.ToTable("ban", (string)null);
                });

            modelBuilder.Entity("Domain.Discussions.Discussion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created_utc");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("UserCreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("user_created_by");

                    b.Property<Guid>("UserCreatedByNavigationId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserCreatedBy");

                    b.HasIndex("UserCreatedByNavigationId");

                    b.ToTable("discussion", (string)null);
                });

            modelBuilder.Entity("Domain.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("contents");

                    b.Property<DateTimeOffset>("DateSentUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_sent_utc");

                    b.Property<Guid>("DiscussionId")
                        .HasColumnType("uuid")
                        .HasColumnName("discussion_id");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("boolean")
                        .HasColumnName("is_edited");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("DiscussionId");

                    b.HasIndex("UserId");

                    b.ToTable("message", (string)null);
                });

            modelBuilder.Entity("Domain.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("DiscussionId")
                        .HasColumnType("uuid")
                        .HasColumnName("discussion_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.Property<IEnumerable<string>>("Permissions")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("DiscussionId");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AboutSection")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("about_section");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created_utc");

                    b.Property<ReadOnlyCollection<Guid>>("Discussions")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<ReadOnlyCollection<Guid>>("Roles")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("is_deleted = FALSE");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("is_deleted = FALSE");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("Domain.Bans.Ban", b =>
                {
                    b.HasOne("Domain.Discussions.Discussion", "DiscussionNavigation")
                        .WithMany("BansNavigation")
                        .HasForeignKey("DiscussionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Users.User", "UserNavigation")
                        .WithMany("BansNavigation")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscussionNavigation");

                    b.Navigation("UserNavigation");
                });

            modelBuilder.Entity("Domain.Discussions.Discussion", b =>
                {
                    b.HasOne("Domain.Users.User", null)
                        .WithMany("DiscussionsNavigation")
                        .HasForeignKey("UserCreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Users.User", "UserCreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("UserCreatedByNavigationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserCreatedByNavigation");
                });

            modelBuilder.Entity("Domain.Messages.Message", b =>
                {
                    b.HasOne("Domain.Discussions.Discussion", "DiscussionNavigation")
                        .WithMany("MessagesNavigation")
                        .HasForeignKey("DiscussionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Users.User", "UserNavigation")
                        .WithMany("MessagesNavigation")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscussionNavigation");

                    b.Navigation("UserNavigation");
                });

            modelBuilder.Entity("Domain.Roles.Role", b =>
                {
                    b.HasOne("Domain.Discussions.Discussion", "DiscussionNavigation")
                        .WithMany("RolesNavigation")
                        .HasForeignKey("DiscussionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscussionNavigation");
                });

            modelBuilder.Entity("Domain.Discussions.Discussion", b =>
                {
                    b.Navigation("BansNavigation");

                    b.Navigation("MessagesNavigation");

                    b.Navigation("RolesNavigation");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Navigation("BansNavigation");

                    b.Navigation("DiscussionsNavigation");

                    b.Navigation("MessagesNavigation");
                });
#pragma warning restore 612, 618
        }
    }
}
