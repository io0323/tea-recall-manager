using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeaRecallManager.Models;
using TeaRecallManager.Repositories;

namespace TeaRecallManager.ViewModels
{
  /* ロット一覧画面向けの ViewModel。検索・読み込みを提供する */
  public class LotListViewModel : BaseViewModel
  {
    private readonly ITeaLotRepository _repo = new TeaLotRepository();

    public ObservableCollection<TeaLot> Lots { get; } = new();

    private string? _searchQuery;
    public string? SearchQuery
    {
      get => _searchQuery;
      set
      {
        _searchQuery = value;
        RaisePropertyChanged();
      }
    }

    public LotListViewModel()
    {
      // コンストラクタで非同期初期化はしない（呼び出し元で await する）
    }

    /* ロットを読み込む。LINQで検索条件を適用する。 */
    public async Task LoadAsync()
    {
      var list = await _repo.SearchAsync(SearchQuery);
      Lots.Clear();
      foreach (var l in list) Lots.Add(l);
    }
  }
}

