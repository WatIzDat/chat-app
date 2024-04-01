using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_to_added_role_role_RolesNavigationId",
                table: "user_to_added_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_to_added_role_user_UsersNavigationId",
                table: "user_to_added_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_to_joined_discussion_discussion_JoinedDiscussionsNavig~",
                table: "user_to_joined_discussion");

            migrationBuilder.DropForeignKey(
                name: "FK_user_to_joined_discussion_user_JoinedUsersNavigationId",
                table: "user_to_joined_discussion");

            migrationBuilder.RenameColumn(
                name: "JoinedUsersNavigationId",
                table: "user_to_joined_discussion",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "JoinedDiscussionsNavigationId",
                table: "user_to_joined_discussion",
                newName: "discussion_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_to_joined_discussion_JoinedUsersNavigationId",
                table: "user_to_joined_discussion",
                newName: "IX_user_to_joined_discussion_user_id");

            migrationBuilder.RenameColumn(
                name: "UsersNavigationId",
                table: "user_to_added_role",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RolesNavigationId",
                table: "user_to_added_role",
                newName: "role_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_to_added_role_UsersNavigationId",
                table: "user_to_added_role",
                newName: "IX_user_to_added_role_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_added_role_role_role_id",
                table: "user_to_added_role",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_added_role_user_user_id",
                table: "user_to_added_role",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_joined_discussion_discussion_discussion_id",
                table: "user_to_joined_discussion",
                column: "discussion_id",
                principalTable: "discussion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_joined_discussion_user_user_id",
                table: "user_to_joined_discussion",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_to_added_role_role_role_id",
                table: "user_to_added_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_to_added_role_user_user_id",
                table: "user_to_added_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_to_joined_discussion_discussion_discussion_id",
                table: "user_to_joined_discussion");

            migrationBuilder.DropForeignKey(
                name: "FK_user_to_joined_discussion_user_user_id",
                table: "user_to_joined_discussion");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_to_joined_discussion",
                newName: "JoinedUsersNavigationId");

            migrationBuilder.RenameColumn(
                name: "discussion_id",
                table: "user_to_joined_discussion",
                newName: "JoinedDiscussionsNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_user_to_joined_discussion_user_id",
                table: "user_to_joined_discussion",
                newName: "IX_user_to_joined_discussion_JoinedUsersNavigationId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_to_added_role",
                newName: "UsersNavigationId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "user_to_added_role",
                newName: "RolesNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_user_to_added_role_user_id",
                table: "user_to_added_role",
                newName: "IX_user_to_added_role_UsersNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_added_role_role_RolesNavigationId",
                table: "user_to_added_role",
                column: "RolesNavigationId",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_added_role_user_UsersNavigationId",
                table: "user_to_added_role",
                column: "UsersNavigationId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_joined_discussion_discussion_JoinedDiscussionsNavig~",
                table: "user_to_joined_discussion",
                column: "JoinedDiscussionsNavigationId",
                principalTable: "discussion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_to_joined_discussion_user_JoinedUsersNavigationId",
                table: "user_to_joined_discussion",
                column: "JoinedUsersNavigationId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
