using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaRecallManager.Core.Models;
using TeaRecallManager.Repositories;
using TeaRecallManager.ViewModels;
using Xunit;

namespace TeaRecallManager.Tests
{
  /* LotListViewModel の単体テスト（Repository のフェイク実装を使用） */
  public class LotListViewModelTests
  {
    private class FakeRepo : ITeaLotRepository
    {
      private readonly List<TeaLot> _data;
      public FakeRepo(IEnumerable<TeaLot> seed) => _data = seed.ToList();
      public Task<List<TeaLot>> GetAllAsync() => Task.FromResult(_data.ToList());
      public Task<List<TeaLot>> SearchAsync(string? query)
      {
        if (string.IsNullOrWhiteSpace(query)) return Task.FromResult(_data.ToList());
        var q = query.Trim();
        var r = _data.Where(t => t.LotCode.Contains(q) || t.Origin.Contains(q)).ToList();
        return Task.FromResult(r);
      }
      public Task<TeaLot?> GetByIdAsync(int id) => Task.FromResult(_data.FirstOrDefault(t => t.Id == id));
    }

    [Fact]
    public async Task LoadAsync_loads_all_lots_when_no_query()
    {
      var seed = new[]
      {
        new TeaLot { Id = 1, LotCode = "LOT-001", Origin = "A", Variety = "Sencha" },
        new TeaLot { Id = 2, LotCode = "LOT-002", Origin = "B", Variety = "Gyokuro" }
      };
      var vm = new LotListViewModel(new FakeRepo(seed));
      await vm.LoadAsync();
      Assert.Equal(2, vm.Lots.Count);
    }

    [Fact]
    public async Task LoadAsync_filters_by_query()
    {
      var seed = new[]
      {
        new TeaLot { Id = 1, LotCode = "ABC-001", Origin = "Fujisan", Variety = "Sencha" },
        new TeaLot { Id = 2, LotCode = "XYZ-002", Origin = "Kawahara", Variety = "Gyokuro" }
      };
      var vm = new LotListViewModel(new FakeRepo(seed));
      vm.SearchQuery = "ABC";
      await vm.LoadAsync();
      Assert.Single(vm.Lots);
      Assert.Equal("ABC-001", vm.Lots[0].LotCode);
    }
  }
}

