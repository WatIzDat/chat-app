using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    date_created_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    about_section = table.Column<string>(type: "text", nullable: false),
                    Discussions = table.Column<ReadOnlyCollection<Guid>>(type: "uuid[]", nullable: false),
                    Roles = table.Column<ReadOnlyCollection<Guid>>(type: "uuid[]", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discussion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    date_created_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatedByNavigationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discussion", x => x.id);
                    table.ForeignKey(
                        name: "FK_discussion_user_UserCreatedByNavigationId",
                        column: x => x.UserCreatedByNavigationId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_discussion_user_user_created_by",
                        column: x => x.user_created_by,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ban",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_ban_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_unbanned = table.Column<bool>(type: "boolean", nullable: false),
                    date_of_unban_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_ban_permanent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ban", x => x.id);
                    table.ForeignKey(
                        name: "FK_ban_discussion_discussion_id",
                        column: x => x.discussion_id,
                        principalTable: "discussion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ban_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contents = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    date_sent_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_edited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.id);
                    table.ForeignKey(
                        name: "FK_message_discussion_discussion_id",
                        column: x => x.discussion_id,
                        principalTable: "discussion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_message_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Permissions = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_discussion_discussion_id",
                        column: x => x.discussion_id,
                        principalTable: "discussion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ban_discussion_id",
                table: "ban",
                column: "discussion_id");

            migrationBuilder.CreateIndex(
                name: "IX_ban_user_id",
                table: "ban",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_discussion_UserCreatedByNavigationId",
                table: "discussion",
                column: "UserCreatedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_discussion_user_created_by",
                table: "discussion",
                column: "user_created_by");

            migrationBuilder.CreateIndex(
                name: "IX_message_discussion_id",
                table: "message",
                column: "discussion_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_user_id",
                table: "message",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_discussion_id",
                table: "role",
                column: "discussion_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true,
                filter: "is_deleted = FALSE");

            migrationBuilder.CreateIndex(
                name: "IX_user_username",
                table: "user",
                column: "username",
                unique: true,
                filter: "is_deleted = FALSE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ban");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "discussion");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
