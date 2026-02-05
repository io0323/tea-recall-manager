using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Core.Services;
using TeaRecallManager.Data;
using TeaRecallManager.Core.Models;
using Xunit;

namespace TeaRecallManager.Tests
{
  public class RecallAnalysisTests
  {
    private static DbContextOptions<AppDbContext> CreateOptions(SqliteConnection conn) =>
      new DbContextOptionsBuilder<AppDbContext>().UseSqlite(conn).Options;

    [Fact]
    public async Task AnalyzeAsync_finds_related_lots_and_marks_shipped()
    {
      using var conn = new SqliteConnection("DataSource=:memory:");
      conn.Open();
      var options = CreateOptions(conn);

      using (var db = new AppDbContext(options))
      {
        db.Database.EnsureCreated();
        var seed = new TeaLot { LotCode = "S1", Origin = "A", Variety = "Sencha", HarvestDate = DateTime.Today };
        db.TeaLots.Add(seed);
        db.SaveChanges();

        var other = new TeaLot { LotCode = "R1", Origin = "A", Variety = "Sencha", HarvestDate = DateTime.Today };
        db.TeaLots.Add(other);
        db.SaveChanges();

        db.ProcessEvents.Add(new ProcessEvent { TeaLotId = seed.Id, EventType = "Steaming", OccurredAt = DateTime.Today });
        db.ProcessEvents.Add(new ProcessEvent { TeaLotId = other.Id, EventType = "Rolling", OccurredAt = DateTime.Today });
        db.Shipments.Add(new Shipment { TeaLotId = other.Id, Destination = "Tokyo", ShippedAt = DateTime.Today, QuantityKg = 10 });
        db.SaveChanges();
      }

      using (var db = new AppDbContext(options))
      {
        var service = new RecallAnalysisService(db);
        var result = await service.AnalyzeAsync(1);
        Assert.Single(result.RelatedLots);
        var related = result.RelatedLots.First();
        Assert.True(related.IsShipped);
        Assert.Single(related.Shipments);
      }
    }
  }
}

