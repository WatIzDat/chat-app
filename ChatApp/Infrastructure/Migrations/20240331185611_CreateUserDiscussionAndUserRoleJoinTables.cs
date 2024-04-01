using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserDiscussionAndUserRoleJoinTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discussions",
                table: "user");

            migrationBuilder.DropColumn(
                name: "roles",
                table: "user");

            migrationBuilder.CreateTable(
                name: "user_to_added_role",
                columns: table => new
                {
                    RolesNavigationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersNavigationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_to_added_role", x => new { x.RolesNavigationId, x.UsersNavigationId });
                    table.ForeignKey(
                        name: "FK_user_to_added_role_role_RolesNavigationId",
                        column: x => x.RolesNavigationId,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_to_added_role_user_UsersNavigationId",
                        column: x => x.UsersNavigationId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_to_joined_discussion",
                columns: table => new
                {
                    JoinedDiscussionsNavigationId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedUsersNavigationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_to_joined_discussion", x => new { x.JoinedDiscussionsNavigationId, x.JoinedUsersNavigationId });
                    table.ForeignKey(
                        name: "FK_user_to_joined_discussion_discussion_JoinedDiscussionsNavig~",
                        column: x => x.JoinedDiscussionsNavigationId,
                        principalTable: "discussion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_to_joined_discussion_user_JoinedUsersNavigationId",
                        column: x => x.JoinedUsersNavigationId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_to_added_role_UsersNavigationId",
                table: "user_to_added_role",
                column: "UsersNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_user_to_joined_discussion_JoinedUsersNavigationId",
                table: "user_to_joined_discussion",
                column: "JoinedUsersNavigationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_to_added_role");

            migrationBuilder.DropTable(
                name: "user_to_joined_discussion");

            migrationBuilder.AddColumn<ReadOnlyCollection<Guid>>(
                name: "discussions",
                table: "user",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<ReadOnlyCollection<Guid>>(
                name: "roles",
                table: "user",
                type: "uuid[]",
                nullable: false);
        }
    }
}
