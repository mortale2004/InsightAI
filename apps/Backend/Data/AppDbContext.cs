using Backend.Models;
using Backend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<FileType> FileTypes => Set<FileType>();
        public DbSet<ResponseType> ResponseTypes => Set<ResponseType>();
        public DbSet<Region> Regions => Set<Region>();
        public DbSet<Application> Applications => Set<Application>();
        public DbSet<Prompt> Prompts => Set<Prompt>();
        public DbSet<PromptResponse> PromptResponses => Set<PromptResponse>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserApplicationMapping> UserApplicationMappings => Set<UserApplicationMapping>();
        public DbSet<UserPrompt> UserPrompts => Set<UserPrompt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region SeedData
            // ---------------- ResponseType ----------------
            modelBuilder.Entity<ResponseType>().HasData(
                new ResponseType
                {
                    ResponseTypeId = 1,
                    ResponseTypeName = "TEXT",
                    IsActive = true
                },
                new ResponseType
                {
                    ResponseTypeId = 2,
                    ResponseTypeName = "JSON",
                    IsActive = true
                },
                new ResponseType
                {
                    ResponseTypeId = 3,
                    ResponseTypeName = "HTML",
                    IsActive = true
                }
            );

            // ---------------- Region ----------------
            modelBuilder.Entity<Region>().HasData(
                new Region
                {
                    RegionId = 1,
                    RegionName = "DEVELOPMENT",
                    Description = "Development",
                    IsActive = true,
                    PromptText = "This is a Development Region"
                },
                new Region
                {
                    RegionId = 2,
                    RegionName = "QA",
                    Description = "QA",
                    IsActive = true,
                    PromptText = "This is a QA region"
                }
            );

            // ---------------- Application ----------------
            modelBuilder.Entity<Application>().HasData(
                new Application
                {
                    ApplicationId = 1,
                    ApplicationName = "Xelence 7.0",
                    PromptText = "Xelence 7.0 is a low code no code platform it have entities, rules, forms, inbound, outbound files."

                },
                new Application
                {
                    ApplicationId = 2,
                    ApplicationName = "Xelence 6.0",
                    PromptText = "Xelence 6.0 is a low code no code platform it have entities, rules, forms, inbound, outbound files."

                }
            );

            // ---------------- FileType ----------------
            modelBuilder.Entity<FileType>().HasData(
                new FileType
                {
                    FileTypeId = 1,
                    ApplicationId = 1,
                    FileTypeName = "FORM",
                    Description = "Form Contains Js,HTML,Css",
                    PromptText = "This file type contains JS,HTML,CSS,Queries and Rules"
                }
            );

            // ---------------- Prompt ----------------
            modelBuilder.Entity<Prompt>().HasData(
                new Prompt
                {
                    PromptId = 1,
                    PromptName = "Generate Summary",
                    ApplicationId = 1,
                    FileTypeId = 1,
                    RegionId = 1,
                    PromptText = "Summerize the following form"
                }
            );

            // ---------------- PromptResponse ----------------
            modelBuilder.Entity<PromptResponse>().HasData(
                new PromptResponse
                {
                    PromptResponseId = 1,
                    PromptId = 1,
                    ResponseTypeId = 3
                }
            );

            // ---------------- User ----------------
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@insightai.com",
                    PasswordHash = PasswordHasher.Hash("Admin@123"),
                    RegionId = 1
                }
            );


            #endregion SeedData

            modelBuilder.Entity<UserApplicationMapping>(entity =>
            {
                entity.ToTable("UserApplicationMapping");

                entity.HasKey(e => e.UserApplicationId);

                entity.Property(e => e.UserApplicationId)
                            .UseIdentityColumn();

                entity.HasIndex(e => new { e.UserId, e.ApplicationId })
                            .IsUnique();
            });
        }
    }
}
