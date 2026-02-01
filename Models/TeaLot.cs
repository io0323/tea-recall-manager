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
    public string Status { get; set; } = "Normal";

    // ナビゲーションプロパティ
    public ICollection<ProcessEvent>? ProcessEvents { get; set; }
    public ICollection<Shipment>? Shipments { get; set; }
  }
}

