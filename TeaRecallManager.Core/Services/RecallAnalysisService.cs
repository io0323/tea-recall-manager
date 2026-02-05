using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Core.Models;
using TeaRecallManager.Data;

namespace TeaRecallManager.Core.Services
{
  /* 回収影響分析を行う実装
     - 指定ロットの工程発生日（date component）に一致する他ロットを関連ロットとして抽出
     - 出荷情報があるロットは回収対象としてマークする
  */
  public class RecallAnalysisService : IRecallAnalysisService
  {
    private readonly AppDbContext _db;

    public RecallAnalysisService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<RecallAnalysisResult> AnalyzeAsync(int seedTeaLotId)
    {
      var seed = await _db.TeaLots
        .Include(t => t.ProcessEvents)
        .Include(t => t.Shipments)
        .FirstOrDefaultAsync(t => t.Id == seedTeaLotId);

      if (seed == null) return new RecallAnalysisResult(seedTeaLotId, Array.Empty<RelatedLot>());

      // 起点ロットの工程発生日の集合（日付のみ）
      var seedDates = seed.ProcessEvents?
        .Select(e => e.OccurredAt.Date)
        .Distinct()
        .ToArray() ?? Array.Empty<DateTime>();

      // 同一日に加工された他ロットを抽出
      var relatedLots = await _db.TeaLots
        .Include(t => t.ProcessEvents)
        .Include(t => t.Shipments)
        .Where(t => t.Id != seedTeaLotId &&
                    t.ProcessEvents.Any(pe => seedDates.Contains(pe.OccurredAt.Date)))
        .ToListAsync();

      var results = relatedLots.Select(t => new RelatedLot
      {
        TeaLot = t,
        IsShipped = (t.Shipments != null && t.Shipments.Any()),
        Shipments = t.Shipments?.ToList() ?? new List<Shipment>()
      }).ToList();

      return new RecallAnalysisResult(seedTeaLotId, results);
    }
  }

  public record RecallAnalysisResult(int SeedTeaLotId, IReadOnlyList<RelatedLot> RelatedLots);

  public class RelatedLot
  {
    public TeaLot? TeaLot { get; init; }
    public bool IsShipped { get; init; }
    public List<Shipment> Shipments { get; init; } = new();
  }
}

