using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeaRecallManager.Core.Models;
using TeaRecallManager.Repositories;

namespace TeaRecallManager.ViewModels
{
  /* ロット一覧画面向けの ViewModel。検索・読み込みを提供する */
  public class LotListViewModel : BaseViewModel
  {
    private readonly ITeaLotRepository _repo;

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

    /* DI 可能にするため Repository を注入できるコンストラクタを提供する */
    public LotListViewModel(ITeaLotRepository? repo = null)
    {
      _repo = repo ?? new TeaLotRepository();
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

