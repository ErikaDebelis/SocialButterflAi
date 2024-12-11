using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using SocialButterFlAi.Data.Chat;
using SocialButterFlAi.Data.Identity;
using SocialButterFlAi.Data.Identity;

using SocialButterFlAi.Data.Analysis.Entities;

namespace SocialButterFlAi.Data.Analysis
{
    public class AnalysisDbContext : DbContext
    {
        public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options) : base(options)
        { }

        public DbSet<Video> Videos { get; set; }
        public DbSet<EnhancedCaption> Captions { get; set; }
        public DbSet<Analysis.Entities.Analysis> Analyses { get; set; }

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
                { "VideoId", Guid.Parse("6fb87597-28e6-4cd7-b747-03e5c3f64aec") },
                { "CaptionId", Guid.Parse("3f175ab9-998b-40af-aca0-c21c38273ce7") },
                { "AnalysisId", Guid.Parse("41aea377-95ff-420a-a77c-3b274a1bdc2b") },
            };

            // Create test seed data (for each entity type in the module) initial migration/table generation

            //todo: fix seeded data
            var testVideos = new List<Video>()
            {
                new Video()
                {
                    Id = _Ids["VideoId"],
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
                    Id = _Ids["CaptionId"],
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
                    Id = _Ids["AnalysisId"],
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            // Add Identity and Chat Models (Dependencies) + Ignore Dependant Tables for Migration (they've already been added/created)
            IdentityDbContext.SetupModelNavigation(modelBuilder);
            IdentityDbContext.IgnoreTables(modelBuilder);
            ChatDbContext.SetupModelNavigation(modelBuilder);
            ChatDbContext.IgnoreTables(modelBuilder);

            SetupModelNavigation(modelBuilder);

            // Now seed the data into the database
            modelBuilder.Entity<Video>()
                .HasData(testVideos);

            modelBuilder.Entity<EnhancedCaption>()
                .HasData(testCaptions);

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .HasData(testAnalyses);
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
            var captionType = typeof(EnhancedCaption);
            var analysisModelType = typeof(Analysis.Entities.Analysis);

            //create the entity helper and pass in the model builder and the types and recurse to build the entities
            var entityHelper = new EntityHelper();

            entityHelper.EntityBuilder(modelBuilder, videoType, null);
            entityHelper.EntityBuilder(modelBuilder, captionType, null);
            entityHelper.EntityBuilder(modelBuilder, analysisModelType, null);

            // Set up model relationships for navigation with keys and such
            modelBuilder.Entity<Video>()
                .HasMany(v => v.Captions)
                .WithOne(c => c.Video)
                .HasForeignKey(c => c.VideoId);

            modelBuilder.Entity<EnhancedCaption>()
                .HasMany(c => c.Analyses)
                .WithOne(a => a.Caption)
                .HasForeignKey(a => a.CaptionId);

            modelBuilder.Entity<Analysis.Entities.Analysis>()
                .HasOne(a => a.Video)
                .WithMany()
                .HasForeignKey(a => a.VideoId);
        }
        #endregion
    }

    /// <summary>
    ///create a new instance of the AnalysisDbContext
    /// </summary>
    public class AnalysisDbContextFactory : IDesignTimeDbContextFactory<AnalysisDbContext>
    {
        private string _connectionString;

        /// <summary>
        ///
        /// </summary>
        public AnalysisDbContextFactory()
        {
            _connectionString = "Host=postgres.socialbutterflai;Port=5434;Database=CueCoach;Username=postgres;Password=postgres;Include Error Detail=true";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        public AnalysisDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        ///
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