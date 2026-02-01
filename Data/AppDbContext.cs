using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Models;

namespace TeaRecallManager.Data
{
  /* EF Core の DbContext を定義する */
  public class AppDbContext : DbContext
  {
    public DbSet<TeaLot> TeaLots => Set<TeaLot>();
    public DbSet<ProcessEvent> ProcessEvents => Set<ProcessEvent>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=tearecall.db");
    }
  }
}

