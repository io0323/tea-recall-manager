using System;

namespace TeaRecallManager.Models
{
  /* 出荷履歴を表すエンティティ */
  public class Shipment
  {
    public int Id { get; set; }
    public int TeaLotId { get; set; }
    public string Destination { get; set; } = null!;
    public DateTime ShippedAt { get; set; }
    public double QuantityKg { get; set; }

    public TeaLot? TeaLot { get; set; }
  }
}

