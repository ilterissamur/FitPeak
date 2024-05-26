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
    }
}
