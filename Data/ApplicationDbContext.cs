using Microsoft.EntityFrameworkCore;
using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Claims)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "admin123", // In production, this should be hashed
                    Role = "Admin",
                    Email = "admin@university.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    IsActive = true,
                    DateCreated = DateTime.Now
                }
            );
        }
    }
}