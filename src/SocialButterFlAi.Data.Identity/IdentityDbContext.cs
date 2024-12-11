using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using SocialButterFlAi.Data.Identity;
using SocialButterFlAi.Data.Identity.Entities;

namespace SocialButterFlAi.Data.Identity
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        { }

        public DbSet<Identity.Entities.Identity> Identities { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<PronounChoice> PronounChoices { get; set; }
        public DbSet<ProfilePronounChoice> ProfilePronounChoices { get; set; }

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
            // Set up model relationships for navigation with keys and such
            SetupModelNavigation(modelBuilder);

            // Set up dictionary of model ids for seeding
            _Ids = new Dictionary<string, Guid>()
            {
                { "Identity1Id", Guid.Parse("513227da-56e9-4ac8-9c82-857a55581ffe") },
                { "Identity2Id", Guid.Parse("9c29abd3-4028-4081-878c-44b6bfb0e8d3") },
                { "ProfileId",  Guid.Parse("5bb912ff-bcc4-4d1e-99b3-064723806d35") },
                { "PronounChoice1Id",  Guid.Parse("43dbd2e6-3e0c-40b7-9299-967be8df17b9") },
                { "PronounChoice2Id",  Guid.Parse("77a03575-73a6-4013-97eb-69f6114c24c2") },
                { "PronounChoice3Id",  Guid.Parse("ad01cee5-308f-4f3f-9845-547d1ab2ddac") },
                { "PronounChoice4Id",  Guid.Parse("eb8923bf-2a2a-47b7-b64b-6b1b1778b1bc") },
                { "PronounChoice5Id",  Guid.Parse("f4228212-0e19-4b01-bc63-c58948086865") },
                { "PronounChoice6Id",  Guid.Parse("c285fa13-edb6-4c35-99e0-0fe0edd6b24d") }
            };

            // Create test seed data (for each entity type in the module) initial migration/table generation
            var testIdentity = new List<Entities.Identity>()
            {
                new Entities.Identity()
                {
                    Id = _Ids["Identity1Id"],
                    StartDate = DateTime.UtcNow,
                    Enabled = true,
                    Name = "Test Identity 1",
                    Email = "test@test.com",
                    Password = "hashedPassword",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new Entities.Identity()
                {
                    Id = _Ids["Identity2Id"],
                    StartDate = DateTime.UtcNow,
                    Enabled = true,
                    Name = "Test Identity 2",
                    Email = "anotheremail@test.com",
                    Password = "hashedPassword",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                }
            };

            var testProfile = new Profile()
            {
                Id = _Ids["ProfileId"],
                IdentityId = _Ids["Identity1Id"],
                Name = "Test Profile",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe",
                Title = Title.Dr,
                Gender = Gender.Male,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            var testPronounChoices = new List<PronounChoice>()
            {
                new PronounChoice()
                {
                    Id = _Ids["PronounChoice1Id"],
                    Pronoun = Pronoun.He,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new PronounChoice()
                {
                    Id = _Ids["PronounChoice2Id"],
                    Pronoun = Pronoun.Him,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new PronounChoice()
                {
                    Id = _Ids["PronounChoice3Id"],
                    Pronoun = Pronoun.She,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new PronounChoice()
                {
                    Id = _Ids["PronounChoice4Id"],
                    Pronoun = Pronoun.Her,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new PronounChoice()
                {
                    Id = _Ids["PronounChoice5Id"],
                    Pronoun = Pronoun.They,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new PronounChoice()
                {
                    Id = _Ids["PronounChoice6Id"],
                    Pronoun = Pronoun.Them,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                }
            };

            var testProfilePronounChoices = new List<ProfilePronounChoice>()
            {
                new ProfilePronounChoice()
                {
                    ProfileId = _Ids["ProfileId"],
                    PronounChoiceId = _Ids["PronounChoice1Id"],
                },
                new ProfilePronounChoice()
                {
                    ProfileId = _Ids["ProfileId"],
                    PronounChoiceId = _Ids["PronounChoice2Id"],
                }
            };

            // Now seed the data into the database
            modelBuilder.Entity<Entities.Identity>()
                .HasData(testIdentity);

            modelBuilder.Entity<Profile>()
                .HasData(testProfile);

            modelBuilder.Entity<PronounChoice>()
                .HasData(testPronounChoices);

            modelBuilder.Entity<ProfilePronounChoice>()
                .HasData(testProfilePronounChoices);
        }
        #endregion

        #region IgnoreTables Helper Method
        /// <summary>
        /// IgnoreTables is used to ignore tables that are not needed in the DbContext
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void IgnoreTables(
            ModelBuilder modelBuilder
        )
        {
            modelBuilder.Entity<Identity.Entities.Identity>()
                .ToTable(
                    nameof(Identity),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<Profile>()
                .ToTable(
                    nameof(Profile),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<PronounChoice>()
                .ToTable(
                    nameof(PronounChoice),
                    t => t.ExcludeFromMigrations()
                );

            modelBuilder.Entity<ProfilePronounChoice>()
                .ToTable(
                    nameof(ProfilePronounChoice),
                    t => t.ExcludeFromMigrations()
                );
        }
        #endregion

        #region SetupModelNavigation Helper Method
        /// <summary>
        /// SetupModelNavigation is used to set up the navigation properties for the entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void SetupModelNavigation(
            ModelBuilder modelBuilder
        )
		{
            // create the type variables for the entities
            var identityModelType = typeof(Entities.Identity);
            var profileType = typeof(Profile);
            var pronounChoiceType = typeof(PronounChoice);
            var profilePronounChoiceType = typeof(ProfilePronounChoice);

            // create the entity helper and pass in the model builder and the types and recurse to build the entities
            var entityHelper = new EntityHelper();

            entityHelper.EntityBuilder(modelBuilder, identityModelType, null);
            entityHelper.EntityBuilder(modelBuilder, profileType, null);
            entityHelper.EntityBuilder(modelBuilder, pronounChoiceType, null);
            entityHelper.EntityBuilder(modelBuilder, profilePronounChoiceType, null);

            // Set up Object Shapes and Defaults
            modelBuilder.Entity<Profile>()
                .HasOne(x => x.Identity)
                .WithMany()
                .HasForeignKey(x => x.Id);

            modelBuilder.Entity<Profile>()
                .HasMany(model => model.Pronouns)
                .WithMany()
                .UsingEntity<ProfilePronounChoice>(
                    a => a
                        .HasOne(pc => pc.PronounChoice)
                        .WithMany()
                        .HasForeignKey(x => x.PronounChoiceId),
                    b => b
                        .HasOne(p => p.Profile)
                        .WithMany()
                        .HasForeignKey(x => x.ProfileId)
                )
                .HasKey(x => new { x.PronounChoiceId, x.ProfileId });
		}
        #endregion
    }

    /// <summary>
    /// creates a new instance of the IdentityDbContext
    /// </summary>
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        private string _connectionString;

        /// <summary>
        /// Default Constructor- no parameters- uses default connection string
        /// </summary>
        public IdentityDbContextFactory()
        {
            _connectionString = "Host=postgres.socialbutterflai;Port=5434;Database=CueCoach;Username=postgres;Password=postgres;Include Error Detail=true";
        }

        /// <summary>
        /// Constructor- takes in a connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public IdentityDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Constructor- takes in an args array
        /// </summary>
        /// <param name="args"></param>
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            builder.UseNpgsql(_connectionString);
            builder.EnableSensitiveDataLogging();
            return new IdentityDbContext(builder.Options);
        }
    }
}