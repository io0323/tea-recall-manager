using System;
using System.Collections.Generic;

namespace TeaRecallManager.Models
{
  /* 茶葉ロットを表すエンティティ */
  public class TeaLot
  {
    public int Id { get; set; }
    public string LotCode { get; set; } = null!;
    public string Origin { get; set; } = null!;
    public string Variety { get; set; } = null!;
    public DateTime HarvestDate { get; set; }
    // 状態は列挙型で管理し、DB上は文字列として保持する（AppDbContextで変換を設定）
    public TeaLotStatus Status { get; set; } = TeaLotStatus.Normal;

    // ナビゲーションプロパティ
    public ICollection<ProcessEvent>? ProcessEvents { get; set; }
    public ICollection<Shipment>? Shipments { get; set; }
  }
}

