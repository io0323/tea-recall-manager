using System;
using System.Windows;
using TeaRecallManager.ViewModels;

namespace TeaRecallManager.Views
{
  /* ロット一覧のメインウィンドウ。View と ViewModel をつなぐ */
  public partial class MainWindow : Window
  {
    private readonly LotListViewModel _vm = new();

    public MainWindow()
    {
      InitializeComponent();
      DataContext = _vm;

      Loaded += async (s, e) =>
      {
        await _vm.LoadAsync();
        LotGrid.ItemsSource = _vm.Lots;
      };

      SearchBtn.Click += async (s, e) =>
      {
        _vm.SearchQuery = SearchBox.Text;
        await _vm.LoadAsync();
      };
    }
  }
}

