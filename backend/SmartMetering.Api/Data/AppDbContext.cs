using Microsoft.EntityFrameworkCore;
using SmartMetering.Api.Models;

namespace SmartMetering.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Meter> Meters => Set<Meter>();
    public DbSet<Reading> Readings => Set<Reading>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meter>()
            .HasMany(m => m.Readings)
            .WithOne(r => r.Meter)
            .HasForeignKey(r => r.MeterId);

        modelBuilder.Entity<Meter>()
            .HasMany(m => m.Alerts)
            .WithOne(a => a.Meter)
            .HasForeignKey(a => a.MeterId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
