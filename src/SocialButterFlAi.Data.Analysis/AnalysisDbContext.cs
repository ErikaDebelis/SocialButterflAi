using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Identity;
using SocialButterflAi.Data.Identity;

using SocialButterflAi.Data.Analysis.Entities;

namespace SocialButterflAi.Data.Analysis
{
    public class AnalysisDbContext : DbContext
    {
        public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options) : base(options)
        { }

        public DbSet<Video> Videos { get; set; }
        public DbSet<EnhancedCaption> Captions { get; set; }
        public DbSet<Analysis.Entities.Image> Images { get; set; }
        public DbSet<Analysis.Entities.Audio> Audios { get; set; }
        public DbSet<Analysis.Entities.Analysis> Analyses { get; set; }
        public DbSet<Analysis.Entities.Tone> Tones { get; set; }
        public DbSet<Analysis.Entities.> Intents { get; set; }

        private Dictionary<string, Guid> _Ids;

        #region OnModelCreating
        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {
            // Set up dictionary of model ids for seeding
            //Todo: changes these to constants rather than new guids
            _Ids = new Dictionary<string, Guid>()
            {
                //keep the same id for the identity so that the seed data can be used for testing
                { "Identity1Id", Guid.Parse("513227da-56e9-4ac8-9c82-857a55581ffe")},
                { "Identity2Id", Guid.Parse("9c29abd3-4028-4081-878c-44b6bfb0e8d3")},
                { "Message1Id",  Guid.Parse("579a2981-4a53-4801-8c86-0b543a90ff09") },
                { "Message2Id",  Guid.Parse("5103543e-c959-11ef-be94-5ff2ce75ed71") },
                { "Message3Id",  Guid.Parse("6059ecce-c959-11ef-8e2c-1556ca6766f7") },
                { "VideoId", Guid.Parse("6fb87597-28e6-4cd7-b747-03e5c3f64aec") },
                { "ImageId", Guid.Parse("d88da8d2-c6fe-11ef-92e8-08c7f780046d") },
                { "AudioId", Guid.Parse("6a353bf0-c959-11ef-b07b-577712103d2c") },
                { "CaptionId", Guid.Parse("3f175ab9-998b-40af-aca0-c21c38273ce7") },
                { "Analysis1Id", Guid.Parse("41aea377-95ff-420a-a77c-3b274a1bdc2b") },
                { "Analysis2Id", Guid.Parse("d6c6c224-c6fe-11ef-a402-5a0901f2d5ba") },
                { "Analysis3Id", Guid.Parse("70424177-c959-11ef-b781-f79868ee873a") },
                { "Tone1Id", Guid.Parse("") },
                { "Tone2Id", Guid.Parse("") },
                { "Intent1Id", Guid.Parse("") },
                { "Intent2Id", Guid.Parse("") },
            };

            // Create test seed data (for each entity type in the module) initial migration/table generation

            //todo: fix seeded data
            var testVideos = new List<Video>()
            {
                new Video()
                {
                    IdentityId = _Ids["Identity1Id"],
                    MessageId = _Ids["Message1Id"],
                    Id = _Ids["VideoId"],
                    Title = "cats video",
                    Description = "",
                    Path = "",
                    VideoType = VideoType.mp4,
                    Base64 = SampleData.Base64Video,
                    Duration = TimeSpan.FromSeconds(10),
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testImages = new List<Image>()
            {
                new Image()
                {
                    IdentityId = _Ids["Identity1Id"],
                    MessageId = _Ids["Message2Id"],
                    Id = _Ids["ImageId"],
                    Title = "cat",
                    Description = "",
                    Path = "",
                    Base64 = SampleData.Base64Image,
                    Type = ImageType.jpeg,
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testAudios = new List<Audio>()
            {
                new Audio()
                {
                    IdentityId = _Ids["Identity1Id"],
                    MessageId = _Ids["Message3Id"],
                    Id = _Ids["AudioId"],
                    Base64 = SampleData.Base64Audio,
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testCaptions = new List<EnhancedCaption>()
            {
                new EnhancedCaption()
                {
                    VideoId = _Ids["VideoId"],
                    Id = _Ids["CaptionId"],
                    StartTime = TimeSpan.MinValue.Add(TimeSpan.FromSeconds(0)),
                    EndTime = TimeSpan.MinValue.Add(TimeSpan.FromSeconds(10)),
                    StandardText = "",
                    BackgroundContext = "",
                    SoundEffects = "",
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testAnalyses = new List<Analysis.Entities.Analysis>()
            {
                new Analysis.Entities.Analysis()
                {
                    Id = _Ids["Analysis1Id"],
                    CaptionId = _Ids["CaptionId"],
                    Certainty = 100,
                    EnhancedDescription = "",
                    EmotionalContext = "",
                    NonVerbalCues = "",
                    Metadata = new Dictionary<string, string>(),
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Analysis.Entities.Analysis()
                {
                    Id = _Ids["Analysis2Id"],
                    Certainty = 100,
                    EnhancedDescription = "",
                    EmotionalContext = "",
                    NonVerbalCues = "",
                    Metadata = new Dictionary<string, string>(),
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Analysis.Entities.Analysis()
                {
                    Id = _Ids["Analysis3Id"],
                    Certainty = 100,
                    EnhancedDescription = "",
                    EmotionalContext = "",
                    NonVerbalCues = "",
                    Metadata = new Dictionary<string, string>(),
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testTones = new List<Tone>()
            {
                new()
                {
                    Id = _Ids["Tone1Id"],
                    AnalysisId = _Ids["Analysis1Id"],
                    PrimaryEmotion = "hurt",
                    EmotionalSpectrum = new Dictionary<string, double>()
                    {
                        { "anger", 0.5 },
                        { "joy", 0.3 },
                        { "sadness", 0.2 }
                    },
                    EmotionalContext = "their voice quivered",
                    NonVerbalCues = "they were frowning",
                    IntensityScore = 0.8,
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new()
                {
                    Id = _Ids["Tone2Id"],
                    AnalysisId = _Ids["Analysis2Id"],
                    PrimaryEmotion = "remorse",
                    EmotionalSpectrum = new Dictionary<string, double>()
                    {
                        { "self-conscious", 0.6 },
                        { "annoyance", 0.4 }
                    },
                    EmotionalContext = "the speaker is uncomfortable",
                    NonVerbalCues = "the speaker's eyes darted around",
                    IntensityScore = 0.6,
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testIntents = new List<Intent>()
            {
                new()
                {
                    Id = _Ids["Intent1Id"],
                    AnalysisId = _Ids["Analysis1Id"],
                    PrimaryIntent = "convince them to buy a car",
                    SecondaryIntents = new Dictionary<string, double>()
                    {
                        { "make them feel good", 0.5 },
                        { "make them feel safe", 0.3 }
                    },
                    CertaintyScore = 0.8,
                    SubtextualMeaning = "they are working hard to make a sale",
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new()
                {
                    Id = _Ids["Intent2Id"],
                    AnalysisId = _Ids["Analysis2Id"],
                    PrimaryIntent = "to gain their forgiveness",
                    SecondaryIntents = new Dictionary<string, double>()
                    {
                        { "alleviate the tension", 0.5 }
                    },
                    CertaintyScore = 0.4,
                    SubtextualMeaning = "the speaker is trying to seem polite",
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            }

            // Add Identity and Chat Models (Dependencies) + Ignore Dependant Tables for Migration (they've already been added/created)
            IdentityDbContext.SetupModelNavigation(modelBuilder);
            IdentityDbContext.IgnoreTables(modelBuilder);

            ChatDbContext.SetupModelNavigation(modelBuilder);
            ChatDbContext.IgnoreTables(modelBuilder);

            SetupModelNavigation(modelBuilder);

            // Now seed the data into the database
            modelBuilder.Entity<Video>()
                .HasData(testVideos);

            modelBuilder.Entity<Image>()
                .HasData(testImages);

            modelBuilder.Entity<Audio>()
                .HasData(testAudios);

            modelBuilder.Entity<EnhancedCaption>()
                .HasData(testCaptions);

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .HasData(testAnalyses);

            modelBuilder.Entity<Analysis.Entities.Tone>()
                .HasData(testTones);

            modelBuilder.Entity<Analysis.Entities.Intent>()
                .HasData(testIntents);
        }
        #endregion

        #region IgnoreTables
        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void IgnoreTables(
            ModelBuilder modelBuilder
        )
        {
            modelBuilder.Entity<Video>()
                .ToTable(
                    nameof(Video),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<Image>()
                .ToTable(
                    nameof(Image),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<Audio>()
                .ToTable(
                    nameof(Audio),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<EnhancedCaption>()
                .ToTable(
                    nameof(EnhancedCaption),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .ToTable(
                    nameof(Analysis),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<Analysis.Entities.Tone>()
                .ToTable(
                    nameof(Tone),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<Analysis.Entities.Intent>()
                .ToTable(
                    nameof(Intent),
                    t => t.ExcludeFromMigrations()
                );
        }
        #endregion

        #region SetupModelNavigation
        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void SetupModelNavigation(
            ModelBuilder modelBuilder
        )
        {
            //create the type variables for the entities
            var videoType = typeof(Video);
            var imageType = typeof(Image);
            var audioType = typeof(Audio);
            var captionType = typeof(EnhancedCaption);
            var analysisModelType = typeof(Analysis.Entities.Analysis);
            var toneType = typeof(Analysis.Entities.Tone);
            var intentType = typeof(Analysis.Entities.Intent);

            //create the entity helper and pass in the model builder and the types and recurse to build the entities
            var entityHelper = new EntityHelper();

            entityHelper.EntityBuilder(modelBuilder, videoType, null);
            entityHelper.EntityBuilder(modelBuilder, imageType, null);
            entityHelper.EntityBuilder(modelBuilder, audioType, null);
            entityHelper.EntityBuilder(modelBuilder, captionType, null);
            entityHelper.EntityBuilder(modelBuilder, analysisModelType, null);
            entityHelper.EntityBuilder(modelBuilder, toneType, null);
            entityHelper.EntityBuilder(modelBuilder, intentType, null);

            // Set up model relationships for navigation with keys and such
            modelBuilder.Entity<Video>()
                .HasMany(v => v.Captions)
                .WithOne(c => c.Video)
                .HasForeignKey(c => c.VideoId);

            modelBuilder.Entity<Video>()
                .HasOne(i => i.Identity)
                .WithMany()
                .HasForeignKey(i => i.IdentityId);

            modelBuilder.Entity<Video>()
                .HasOne(i => i.Message)
                .WithMany()
                .HasForeignKey(i => i.MessageId);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Message)
                .WithMany()
                .HasForeignKey(i => i.MessageId);

            modelBuilder.Entity<Image>()
                .HasMany(i => i.Analyses)
                .WithOne();

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Identity)
                .WithMany()
                .HasForeignKey(i => i.IdentityId);

            modelBuilder.Entity<Audio>()
                .HasOne(i => i.Message)
                .WithMany()
                .HasForeignKey(i => i.MessageId);

            modelBuilder.Entity<Audio>()
                .HasMany(v => v.Captions)
                .WithOne(a => a.Audio)
                .HasForeignKey(a => a.AudioId);

            modelBuilder.Entity<Audio>()
                .HasOne(i => i.Identity)
                .WithMany()
                .HasForeignKey(i => i.IdentityId);

            modelBuilder.Entity<EnhancedCaption>()
                .HasMany(c => c.Analyses)
                .WithOne(a => a.Caption)
                .HasForeignKey(a => a.CaptionId);

            modelBuilder.Entity<EnhancedCaption>()
                .HasOne(v => v.Video)
                .WithMany()
                .HasForeignKey(v => v.VideoId);

            modelBuilder.Entity<EnhancedCaption>()
                .HasOne(a => a.Audio)
                .WithMany()
                .HasForeignKey(a => a.AudioId);

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .HasOne(a => a.Caption)
                .WithMany()
                .HasForeignKey(a => a.CaptionId);

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .HasOne(a => a.Tone)
                .WithOne(t => t.Analysis)
                .HasForeignKey<Tone>(t => t.AnalysisId);

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .HasOne(a => a.Intent)
                .WithOne(i => i.Analysis)
                .HasForeignKey<Intent>(i => i.AnalysisId);
        }
        #endregion
    }

    /// <summary>
    ///create a new instance of the AnalysisDbContext
    /// </summary>
    public class AnalysisDbContextFactory : IDesignTimeDbContextFactory<AnalysisDbContext>
    {
        private readonly string _connectionString;

        /// <summary>
        /// invoked from the program.cs file to create a new instance of the AnalysisDbContextFactory
        /// </summary>
        /// <param name="connectionString"></param>
        public AnalysisDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// invoked from the program.cs file to create a new instance of the AnalysisDbContext
        /// </summary>
        /// <param name="args"></param>
        public AnalysisDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AnalysisDbContext>();
            builder.UseNpgsql(_connectionString);
            return new AnalysisDbContext(builder.Options);
        }
    }
}