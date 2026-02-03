using System;

namespace TeaRecallManager.Core.Models
{
  /* ロットに紐づく工程イベント */
  public class ProcessEvent
  {
    public int Id { get; set; }
    public int TeaLotId { get; set; }
    public string EventType { get; set; } = null!; // Steaming / Rolling / Drying / Packing
    public DateTime OccurredAt { get; set; }

    // ナビゲーション
    public TeaLot? TeaLot { get; set; }
  }
}

