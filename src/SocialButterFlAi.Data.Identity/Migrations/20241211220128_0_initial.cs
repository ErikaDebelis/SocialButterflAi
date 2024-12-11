using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialButterflAi.Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class _0_initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PronounChoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Pronoun = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PronounChoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    PreferredName = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_Identity_Id",
                        column: x => x.Id,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePronounChoices",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    PronounChoiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePronounChoices", x => new { x.PronounChoiceId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProfilePronounChoices_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfilePronounChoices_PronounChoice_PronounChoiceId",
                        column: x => x.PronounChoiceId,
                        principalTable: "PronounChoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Identity",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Email", "Enabled", "ModifiedBy", "ModifiedOn", "Name", "Password", "StartDate", "StopDate" },
                values: new object[,]
                {
                    { new Guid("513227da-56e9-4ac8-9c82-857a55581ffe"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9146), "test@test.com", true, "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9147), "Test Identity 1", "hashedPassword", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9136), null },
                    { new Guid("9c29abd3-4028-4081-878c-44b6bfb0e8d3"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9173), "anotheremail@test.com", true, "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9173), "Test Identity 2", "hashedPassword", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9170), null }
                });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "FirstName", "Gender", "IdentityId", "LastName", "MiddleName", "ModifiedBy", "ModifiedOn", "Name", "PreferredName", "Status", "Title" },
                values: new object[] { new Guid("5bb912ff-bcc4-4d1e-99b3-064723806d35"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9190), "John", "Male", new Guid("513227da-56e9-4ac8-9c82-857a55581ffe"), "Doe", "Test", "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9190), "Test Profile", null, "Active", "Dr" });

            migrationBuilder.InsertData(
                table: "PronounChoice",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Pronoun" },
                values: new object[,]
                {
                    { new Guid("43dbd2e6-3e0c-40b7-9299-967be8df17b9"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9198), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9199), "He" },
                    { new Guid("77a03575-73a6-4013-97eb-69f6114c24c2"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9205), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9206), "Him" },
                    { new Guid("ad01cee5-308f-4f3f-9845-547d1ab2ddac"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9210), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9211), "She" },
                    { new Guid("c285fa13-edb6-4c35-99e0-0fe0edd6b24d"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9231), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9232), "Them" },
                    { new Guid("eb8923bf-2a2a-47b7-b64b-6b1b1778b1bc"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9216), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9217), "Her" },
                    { new Guid("f4228212-0e19-4b01-bc63-c58948086865"), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9221), "System", new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9222), "They" }
                });

            migrationBuilder.InsertData(
                table: "ProfilePronounChoices",
                columns: new[] { "ProfileId", "PronounChoiceId" },
                values: new object[,]
                {
                    { new Guid("5bb912ff-bcc4-4d1e-99b3-064723806d35"), new Guid("43dbd2e6-3e0c-40b7-9299-967be8df17b9") },
                    { new Guid("5bb912ff-bcc4-4d1e-99b3-064723806d35"), new Guid("77a03575-73a6-4013-97eb-69f6114c24c2") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePronounChoices_ProfileId",
                table: "ProfilePronounChoices",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePronounChoices");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "PronounChoice");

            migrationBuilder.DropTable(
                name: "Identity");
        }
    }
}
