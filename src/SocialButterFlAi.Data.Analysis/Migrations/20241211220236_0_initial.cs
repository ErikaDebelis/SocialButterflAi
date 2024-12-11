using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialButterflAi.Data.Analysis.Migrations
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
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    VideoType = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Video_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Video_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Captions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StandardText = table.Column<string>(type: "text", nullable: true),
                    BackgroundContext = table.Column<string>(type: "text", nullable: true),
                    SoundEffects = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Captions_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CaptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Certainty = table.Column<double>(type: "double precision", nullable: false),
                    EnhancedDescription = table.Column<string>(type: "text", nullable: true),
                    EmotionalContext = table.Column<string>(type: "text", nullable: true),
                    NonVerbalCues = table.Column<string>(type: "text", nullable: true),
                    Metadata = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analyses_Captions_CaptionId",
                        column: x => x.CaptionId,
                        principalTable: "Captions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Analyses_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Analyses",
                columns: new[] { "Id", "CaptionId", "Certainty", "CreatedBy", "CreatedOn", "EmotionalContext", "EnhancedDescription", "Metadata", "ModifiedBy", "ModifiedOn", "NonVerbalCues", "VideoId" },
                values: new object[] { new Guid("41aea377-95ff-420a-a77c-3b274a1bdc2b"), new Guid("00000000-0000-0000-0000-000000000000"), 0.0, "Test", new DateTime(2024, 12, 11, 22, 2, 36, 319, DateTimeKind.Utc).AddTicks(6204), null, null, null, "Test", new DateTime(2024, 12, 11, 22, 2, 36, 319, DateTimeKind.Utc).AddTicks(6205), null, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Captions",
                columns: new[] { "Id", "BackgroundContext", "CreatedBy", "CreatedOn", "EndTime", "ModifiedBy", "ModifiedOn", "SoundEffects", "StandardText", "StartTime", "VideoId" },
                values: new object[] { new Guid("3f175ab9-998b-40af-aca0-c21c38273ce7"), null, "Test", new DateTime(2024, 12, 11, 22, 2, 36, 319, DateTimeKind.Utc).AddTicks(6193), new TimeSpan(0, 0, 0, 0, 0), "Test", new DateTime(2024, 12, 11, 22, 2, 36, 319, DateTimeKind.Utc).AddTicks(6193), null, null, new TimeSpan(0, 0, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Video",
                columns: new[] { "Id", "ChatId", "CreatedBy", "CreatedOn", "Description", "Duration", "IdentityId", "ModifiedBy", "ModifiedOn", "Title", "VideoType", "VideoUrl" },
                values: new object[] { new Guid("6fb87597-28e6-4cd7-b747-03e5c3f64aec"), new Guid("00000000-0000-0000-0000-000000000000"), "Test", new DateTime(2024, 12, 11, 22, 2, 36, 319, DateTimeKind.Utc).AddTicks(6179), null, new TimeSpan(0, 0, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000000"), "Test", new DateTime(2024, 12, 11, 22, 2, 36, 319, DateTimeKind.Utc).AddTicks(6183), null, "unknown", null });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CaptionId",
                table: "Analyses",
                column: "CaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_VideoId",
                table: "Analyses",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captions_VideoId",
                table: "Captions",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Video_ChatId",
                table: "Video",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Video_IdentityId",
                table: "Video",
                column: "IdentityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "Captions");

            migrationBuilder.DropTable(
                name: "Video");
        }
    }
}
