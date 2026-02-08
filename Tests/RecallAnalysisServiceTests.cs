using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Core.Services;
using TeaRecallManager.Core.Models;
using TeaRecallManager.Data;
using Xunit;

namespace Tests
{
  /*
   * RecallAnalysisService の単体テスト
   * - インメモリ SQLite を使って DB を構築し、簡易データを投入して動作検証する
   */
  public class RecallAnalysisServiceTests
  {
    /// <summary>
    /// 簡易データを使って AnalyzeAsync が関連ロットを正しく抽出することを検証する
    /// </summary>
    [Fact]
    public async Task AnalyzeAsync_FindsRelatedLots_BySameProcessDate()
    {
      // in-memory SQLite の接続を作成（同一接続で EnsureCreated を実行する必要がある）
      var connection = new SqliteConnection("DataSource=:memory:");
      await connection.OpenAsync();

      var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(connection)
        .Options;

      // DB 作成とシード
      using (var db = new AppDbContext(options))
      {
        db.Database.EnsureCreated();

        var seedLot = new TeaLot
        {
          LotCode = \"SEED-001\",
          Origin = \"TestFarm\",
          Variety = \"Assam\",
          HarvestDate = DateTime.Today.AddDays(-10)
        };

        var otherLotSameDay = new TeaLot
        {
          LotCode = \"OTHER-001\",
          Origin = \"TestFarm2\",
          Variety = \"Assam\",
          HarvestDate = DateTime.Today.AddDays(-9)
        };

        // 同じ加工日を持つ ProcessEvent を追加
        seedLot.ProcessEvents.Add(new ProcessEvent { EventType = \"Roast\", OccurredAt = DateTime.Today });
        otherLotSameDay.ProcessEvents.Add(new ProcessEvent { EventType = \"Roast\", OccurredAt = DateTime.Today });

        db.TeaLots.Add(seedLot);
        db.TeaLots.Add(otherLotSameDay);
        await db.SaveChangesAsync();
      }

      // テスト本体：サービスで分析を実行
      using (var db = new AppDbContext(options))
      {
        var service = new RecallAnalysisService(db);
        var result = await service.AnalyzeAsync(1); // シードロットは先に挿入した最初の Id=1 を想定

        Assert.NotNull(result);
        Assert.Equal(1, result.RelatedLots.Count); // otherLotSameDay が 1 件見つかるはず
        var related = result.RelatedLots.First();
        Assert.Equal(\"OTHER-001\", related.TeaLot?.LotCode);
      }

      await connection.CloseAsync();
    }
  }
}

