using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TeaRecallManager.Core.Services;
using TeaRecallManager.Core.Models;

namespace TeaRecallManager.ViewModels
{
  /* 
   * 回収影響分析用 ViewModel
   * - 起点ロットIDを受け取り分析を実行する
   * - 業務視点のコメントを付与（関連ロットの抽出と出荷判定）
   */
  public class AnalysisViewModel : BaseViewModel
  {
    private readonly IRecallAnalysisService _service;

    public ObservableCollection<RelatedLotViewModel> RelatedLots { get; } = new();

    private int _seedLotId;
    public int SeedLotId
    {
      get => _seedLotId;
      set
      {
        _seedLotId = value;
        RaisePropertyChanged();
      }
    }

    private bool _isAnalyzing;
    public bool IsAnalyzing
    {
      get => _isAnalyzing;
      private set
      {
        _isAnalyzing = value;
        RaisePropertyChanged();
      }
    }

    public ICommand AnalyzeCommand { get; }

    public AnalysisViewModel(IRecallAnalysisService? service = null)
    {
      // テストや DI を容易にするためコンストラクタ注入を受け付ける
      _service = service ?? throw new ArgumentNullException(nameof(service));
      AnalyzeCommand = new RelayCommand(async () => await AnalyzeAsync(), () => !IsAnalyzing);
    }

    /* 指定した SeedLotId を起点に分析を実行する */
    public async Task AnalyzeAsync()
    {
      if (SeedLotId <= 0) return;
      IsAnalyzing = true;
      try
      {
        var result = await _service.AnalyzeAsync(SeedLotId);
        RelatedLots.Clear();
        foreach (var r in result.RelatedLots)
        {
          RelatedLots.Add(new RelatedLotViewModel(r));
        }
      }
      finally
      {
        IsAnalyzing = false;
      }
    }
  }

  /* 表示用に整形した関連ロット情報 */
  public class RelatedLotViewModel
  {
    public int Id => TeaLot?.Id ?? 0;
    public string LotCode => TeaLot?.LotCode ?? string.Empty;
    public string Origin => TeaLot?.Origin ?? string.Empty;
    public bool IsShipped { get; }
    public int ShipmentsCount { get; }
    public TeaLot? TeaLot { get; }

    public RelatedLotViewModel(Core.Services.RelatedLot model)
    {
      TeaLot = model.TeaLot;
      IsShipped = model.IsShipped;
      ShipmentsCount = model.Shipments?.Count ?? 0;
    }
  }
}

