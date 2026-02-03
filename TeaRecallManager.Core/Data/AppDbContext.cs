using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Core.Models;

namespace TeaRecallManager.Data
{
  /* EF Core の DbContext を定義する */
  public class AppDbContext : DbContext
  {
    public DbSet<TeaLot> TeaLots => Set<TeaLot>();
    public DbSet<ProcessEvent> ProcessEvents => Set<ProcessEvent>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlite("Data Source=tearecall.db");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<TeaLot>()
        .Property(t => t.Status)
        .HasConversion<string>()
        .HasMaxLength(50);
    }
  }
}

