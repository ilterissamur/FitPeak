using FitPeak.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FitPeak.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<ApplicationUserChallenge> ApplicationUserChallenges { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUserChallenge>().HasKey(auc => new { auc.ApplicationUserId, auc.ChallengeId });
        modelBuilder.Entity<ApplicationUserChallenge>().HasOne(auc => auc.ApplicationUser).WithMany(au => au.Challenges).HasForeignKey(auc => auc.ApplicationUserId);
        modelBuilder.Entity<ApplicationUserChallenge>().HasOne(auc => auc.Challenge).WithMany(c => c.Users).HasForeignKey(auc => auc.ChallengeId);

        // modelBuilder.Entity<Challenge>(entity =>
        // {
        //     entity.ToTable("Challenges");

        //     entity.Property(e => e.Id).HasColumnName("id");

        //     entity.Property(e => e.Title)
        //         .HasColumnName("Title")
        //         .HasColumnType("nvarchar(MAX)");

        //     entity.Property(e => e.Description)
        //         .HasColumnName("Description")
        //         .HasColumnType("nvarchar(MAX)");

        //     entity.Property(e => e.Category)
        //         .HasColumnName("Category")
        //         .HasColumnType("nvarchar(MAX)");

        //     entity.Property(e => e.Difficulty)
        //         .HasColumnName("Difficulty")
        //         .HasColumnType("nvarchar(MAX)");

        //     entity.Property(e => e.EndDate)
        //         .HasColumnName("EndDate")
        //         .HasColumnType("datetime2");
        // });

    }
}
