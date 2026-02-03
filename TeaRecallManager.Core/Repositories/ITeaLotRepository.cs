using System.Collections.Generic;
using System.Threading.Tasks;
using TeaRecallManager.Models;

namespace TeaRecallManager.Repositories
{
  /* ロット取得のリポジトリインタフェース */
  public interface ITeaLotRepository
  {
    Task<List<TeaLot>> GetAllAsync();
    Task<List<TeaLot>> SearchAsync(string? query);
    Task<TeaLot?> GetByIdAsync(int id);
  }
}

