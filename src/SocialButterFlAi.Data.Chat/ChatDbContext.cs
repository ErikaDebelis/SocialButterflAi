using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using SocialButterflAi.Data.Identity;
using SocialButterflAi.Data.Chat.Entities;

namespace SocialButterflAi.Data.Chat
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        { }

        public DbSet<Chat.Entities.Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

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
                { "Identity1Id", Guid.Parse("513227da-56e9-4ac8-9c82-857a55581ffe") },
                { "Identity2Id", Guid.Parse("9c29abd3-4028-4081-878c-44b6bfb0e8d3") },
                { "ChatId", Guid.Parse("890e2de0-e3b8-463f-bd16-b12fc754c955") },
                { "MessageId",  Guid.Parse("579a2981-4a53-4801-8c86-0b543a90ff09") },
            };

            // Create test seed data (for each entity type in the module) initial migration/table generation
            var testChats = new List<Entities.Chat>()
            {
                new Entities.Chat()
                {
                    Id = _Ids["ChatId"],
                    Name = "Test Chat",
                    ChatStatus = ChatStatus.Active,
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            var testMessages = new List<Message>()
            {
                new Message()
                {
                    Id = _Ids["MessageId"],
                    ChatId = _Ids["ChatId"],
                    ToIdentityId = _Ids["Identity2Id"],
                    FromIdentityId = _Ids["Identity1Id"],
                    Text = "Test message",
                    MessageType = MessageType.Text,
                    CreatedBy = "Test",
                    ModifiedBy = "Test",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                }
            };

            // Add Identity Models (Dependencies) + Ignore Dependant Tables for Migration (they've already been added/created)
            IdentityDbContext.SetupModelNavigation(modelBuilder);
            IdentityDbContext.IgnoreTables(modelBuilder);

            SetupModelNavigation(modelBuilder);

            // Now seed the data into the database
            modelBuilder.Entity<Entities.Chat>()
                .HasData(testChats);

            modelBuilder.Entity<Message>()
                .HasData(testMessages);
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
            modelBuilder.Entity<Entities.Chat>()
                .ToTable(
                    nameof(Chat),
                    t => t.ExcludeFromMigrations()
                );
            modelBuilder.Entity<Message>()
                .ToTable(
                    nameof(Message),
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
            var chatModelType = typeof(Entities.Chat);
            var messageType = typeof(Message);

            //create the entity helper and pass in the model builder and the types and recurse to build the entities
            var entityHelper = new EntityHelper();

            entityHelper.EntityBuilder(modelBuilder, chatModelType, null);
            entityHelper.EntityBuilder(modelBuilder, messageType, null);

            // Set up model relationships for navigation with keys and such
            modelBuilder.Entity<Entities.Chat>()
                .HasMany(chat => chat.Messages)
                .WithOne(msg => msg.Chat)
                .HasForeignKey(p => p.ChatId);
            
            modelBuilder.Entity<Entities.Chat>()
                .HasMany(model => model.Members)
                .WithMany()
                .UsingEntity<IdentityChat>(
                    a => a
                        .HasOne(ic => ic.Identity)
                        .WithMany()
                        .HasForeignKey(x => x.IdentityId),
                    b => b
                        .HasOne(ic => ic.Chat)
                        .WithMany()
                        .HasForeignKey(x => x.ChatId)
                )
                .HasKey(x => new { x.IdentityId, x.ChatId });

            modelBuilder.Entity<Message>()
                .HasOne(msg => msg.ToIdentity)
                .WithMany()
                .HasForeignKey(p => p.ToIdentityId);

            modelBuilder.Entity<Message>()
                .HasOne(msg => msg.FromIdentity)
                .WithMany()
                .HasForeignKey(p => p.FromIdentityId);
        }
        #endregion
    }

    /// <summary>
    ///create a new instance of the ChatDbContext
    /// </summary>
    public class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
    {
        private readonly string _connectionString;

        /// <summary>
        /// invoked from the program.cs file to create a new instance of the ChatDbContextFactory
        /// </summary>
        /// <param name="connectionString"></param>
        public ChatDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// invoked from the program.cs file to create a new instance of the ChatDbContext
        /// </summary>
        /// <param name="args"></param>
        public ChatDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ChatDbContext>();
            builder.UseNpgsql(_connectionString);
            return new ChatDbContext(builder.Options);
        }
    }
}