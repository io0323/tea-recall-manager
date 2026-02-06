using System;
using System.Windows;
using TeaRecallManager.ViewModels;
using TeaRecallManager.Core.Services;
using TeaRecallManager.Data;

namespace TeaRecallManager.Views
{
  public partial class AnalysisWindow : Window
  {
    private readonly AnalysisViewModel _vm;

    public AnalysisWindow()
    {
      InitializeComponent();

      // 最小限の DI：DbContext を直接作成して Service を渡す（将来的に IoC に差し替え）
      var db = new AppDbContext();
      var service = new RecallAnalysisService(db);
      _vm = new AnalysisViewModel(service);
      DataContext = _vm;

      AnalyzeBtn.Click += async (s, e) =>
      {
        if (int.TryParse(SeedBox.Text, out var id))
        {
          _vm.SeedLotId = id;
          await _vm.AnalyzeAsync();
          RelatedGrid.ItemsSource = _vm.RelatedLots;
        }
      };
    }
  }
}

