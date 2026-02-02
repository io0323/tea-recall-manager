using System;
using System.Linq;
using System.Threading.Tasks;
using TeaRecallManager.Models;

namespace TeaRecallManager.Data
{
  /* 初回起動時にSQLiteを作成し、初期データを投入するユーティリティ */
  public static class DbInitializer
  {
    public static async Task InitializeAsync()
    {
      using var db = new AppDbContext();
      await db.Database.EnsureCreatedAsync();

      if (db.TeaLots.Any())
      {
        return; // 既にシード済み
      }

      var rnd = new Random(42);
      var lots = Enumerable.Range(1, 10).Select(i => new TeaLot
      {
        LotCode = $"LOT-{i:000}",
        Origin = i % 2 == 0 ? "Fujisan Farm" : "Kawahara Farm",
        Variety = i % 3 == 0 ? "Sencha" : "Gyokuro",
        HarvestDate = DateTime.Today.AddDays(-rnd.Next(0, 30)),
        Status = TeaLotStatus.Normal
      }).ToList();

      await db.TeaLots.AddRangeAsync(lots);
      await db.SaveChangesAsync();

      // 各ロットに工程イベント 3〜5件、出荷1〜2件を作成
      foreach (var lot in lots)
      {
        var baseDate = lot.HarvestDate.AddDays(1);
        var eventsCount = rnd.Next(3, 6);
        var events = Enumerable.Range(0, eventsCount).Select(j =>
          new ProcessEvent
          {
            TeaLotId = lot.Id,
            EventType = new[] { "Steaming", "Rolling", "Drying", "Packing" }[j % 4],
            OccurredAt = baseDate.AddHours(j * 6)
          }).ToList();

        await db.ProcessEvents.AddRangeAsync(events);

        var shipmentsCount = rnd.Next(1, 3);
        var shipments = Enumerable.Range(0, shipmentsCount).Select(s =>
          new Shipment
          {
            TeaLotId = lot.Id,
            Destination = s % 2 == 0 ? "Tokyo Distributor" : "Osaka Distributor",
            ShippedAt = baseDate.AddDays(2 + s),
            QuantityKg = Math.Round(10 + rnd.NextDouble() * 90, 2)
          }).ToList();

        await db.Shipments.AddRangeAsync(shipments);
      }

      await db.SaveChangesAsync();
    }
  }
}

