using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialButterflAi.Data.Chat.Migrations
{
    /// <inheritdoc />
    public partial class _0_initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ChatStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToIdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromIdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    Metadata = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Identity_FromIdentityId",
                        column: x => x.FromIdentityId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Identity_ToIdentityId",
                        column: x => x.ToIdentityId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chat",
                columns: new[] { "Id", "ChatStatus", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[] { new Guid("890e2de0-e3b8-463f-bd16-b12fc754c955"), "Active", "Test", new DateTime(2024, 12, 11, 22, 1, 47, 123, DateTimeKind.Utc).AddTicks(1947), "Test", new DateTime(2024, 12, 11, 22, 1, 47, 123, DateTimeKind.Utc).AddTicks(1952), "Test Chat" });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "ChatId", "CreatedBy", "CreatedOn", "FromIdentityId", "MessageType", "Metadata", "ModifiedBy", "ModifiedOn", "Text", "ToIdentityId" },
                values: new object[] { new Guid("579a2981-4a53-4801-8c86-0b543a90ff09"), new Guid("890e2de0-e3b8-463f-bd16-b12fc754c955"), "Test", new DateTime(2024, 12, 11, 22, 1, 47, 123, DateTimeKind.Utc).AddTicks(1961), new Guid("513227da-56e9-4ac8-9c82-857a55581ffe"), "Text", null, "Test", new DateTime(2024, 12, 11, 22, 1, 47, 123, DateTimeKind.Utc).AddTicks(1961), "Test message", new Guid("9c29abd3-4028-4081-878c-44b6bfb0e8d3") });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromIdentityId",
                table: "Messages",
                column: "FromIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToIdentityId",
                table: "Messages",
                column: "ToIdentityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Chat");
        }
    }
}
