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
    /* コンストラクタを追加し、テスト時にオプション注入できるようにする */
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      // テストからオプションが注入される場合は上書きしない
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlite("Data Source=tearecall.db");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // TeaLot.Status を文字列で保存する（可読性向上）
      modelBuilder.Entity<TeaLot>()
        .Property(t => t.Status)
        .HasConversion<string>()
        .HasMaxLength(50);
    }
  }
}

