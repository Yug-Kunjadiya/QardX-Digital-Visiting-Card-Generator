using Microsoft.EntityFrameworkCore;
using QardX.Models;

namespace QardX.Data
{
    public class QardXDbContext : DbContext
    {
        public QardXDbContext(DbContextOptions<QardXDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<VisitingCard> VisitingCards { get; set; }
        public DbSet<CardView> CardViews { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<CustomTemplate> CustomTemplates { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<BatchOperation> BatchOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configure Template entity
            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasKey(e => e.TemplateId);
            });

            // Configure VisitingCard entity
            modelBuilder.Entity<VisitingCard>(entity =>
            {
                entity.HasKey(e => e.CardId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VisitingCards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.VisitingCards)
                    .HasForeignKey(d => d.TemplateId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data for Templates
            modelBuilder.Entity<Template>().HasData(
                new Template { TemplateId = 1, TemplateName = "Classic Blue", FilePath = "/templates/classic-blue.html" },
                new Template { TemplateId = 2, TemplateName = "Minimal White", FilePath = "/templates/minimal-white.html" },
                new Template { TemplateId = 3, TemplateName = "Modern Dark", FilePath = "/templates/modern-dark.html" }
            );
        }
    }
}