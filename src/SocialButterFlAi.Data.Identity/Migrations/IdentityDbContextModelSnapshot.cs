﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialButterflAi.Data.Identity;

#nullable disable

namespace SocialButterflAi.Data.Identity.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    partial class IdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SocialButterflAi.Data.Identity.Entities.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("StopDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Identity");

                    b.HasData(
                        new
                        {
                            Id = new Guid("513227da-56e9-4ac8-9c82-857a55581ffe"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9146),
                            Email = "test@test.com",
                            Enabled = true,
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9147),
                            Name = "Test Identity 1",
                            Password = "hashedPassword",
                            StartDate = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9136)
                        },
                        new
                        {
                            Id = new Guid("9c29abd3-4028-4081-878c-44b6bfb0e8d3"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9173),
                            Email = "anotheremail@test.com",
                            Enabled = true,
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9173),
                            Name = "Test Identity 2",
                            Password = "hashedPassword",
                            StartDate = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9170)
                        });
                });

            modelBuilder.Entity("SocialButterflAi.Data.Identity.Entities.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PreferredName")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Profile");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5bb912ff-bcc4-4d1e-99b3-064723806d35"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9190),
                            FirstName = "John",
                            Gender = "Male",
                            IdentityId = new Guid("513227da-56e9-4ac8-9c82-857a55581ffe"),
                            LastName = "Doe",
                            MiddleName = "Test",
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9190),
                            Name = "Test Profile",
                            Status = "Active",
                            Title = "Dr"
                        });
                });

            modelBuilder.Entity("SocialButterflAi.Data.Identity.Entities.ProfilePronounChoice", b =>
                {
                    b.Property<Guid>("PronounChoiceId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid");

                    b.HasKey("PronounChoiceId", "ProfileId");

                    b.HasIndex("ProfileId");

                    b.ToTable("ProfilePronounChoices");

                    b.HasData(
                        new
                        {
                            PronounChoiceId = new Guid("43dbd2e6-3e0c-40b7-9299-967be8df17b9"),
                            ProfileId = new Guid("5bb912ff-bcc4-4d1e-99b3-064723806d35")
                        },
                        new
                        {
                            PronounChoiceId = new Guid("77a03575-73a6-4013-97eb-69f6114c24c2"),
                            ProfileId = new Guid("5bb912ff-bcc4-4d1e-99b3-064723806d35")
                        });
                });

            modelBuilder.Entity("SocialButterflAi.Data.Identity.Entities.PronounChoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Pronoun")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PronounChoice");

                    b.HasData(
                        new
                        {
                            Id = new Guid("43dbd2e6-3e0c-40b7-9299-967be8df17b9"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9198),
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9199),
                            Pronoun = "He"
                        },
                        new
                        {
                            Id = new Guid("77a03575-73a6-4013-97eb-69f6114c24c2"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9205),
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9206),
                            Pronoun = "Him"
                        },
                        new
                        {
                            Id = new Guid("ad01cee5-308f-4f3f-9845-547d1ab2ddac"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9210),
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9211),
                            Pronoun = "She"
                        },
                        new
                        {
                            Id = new Guid("eb8923bf-2a2a-47b7-b64b-6b1b1778b1bc"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9216),
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9217),
                            Pronoun = "Her"
                        },
                        new
                        {
                            Id = new Guid("f4228212-0e19-4b01-bc63-c58948086865"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9221),
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9222),
                            Pronoun = "They"
                        },
                        new
                        {
                            Id = new Guid("c285fa13-edb6-4c35-99e0-0fe0edd6b24d"),
                            CreatedBy = "System",
                            CreatedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9231),
                            ModifiedBy = "System",
                            ModifiedOn = new DateTime(2024, 12, 11, 22, 1, 27, 859, DateTimeKind.Utc).AddTicks(9232),
                            Pronoun = "Them"
                        });
                });

            modelBuilder.Entity("SocialButterflAi.Data.Identity.Entities.Profile", b =>
                {
                    b.HasOne("SocialButterflAi.Data.Identity.Entities.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("SocialButterflAi.Data.Identity.Entities.ProfilePronounChoice", b =>
                {
                    b.HasOne("SocialButterflAi.Data.Identity.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialButterflAi.Data.Identity.Entities.PronounChoice", "PronounChoice")
                        .WithMany()
                        .HasForeignKey("PronounChoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("PronounChoice");
                });
#pragma warning restore 612, 618
        }
    }
}
