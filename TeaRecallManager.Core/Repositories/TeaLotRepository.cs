using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Data;
using TeaRecallManager.Core.Models;

namespace TeaRecallManager.Repositories
{
  /* EF Core を利用したロットリポジトリ実装 */
  public class TeaLotRepository : ITeaLotRepository
  {
    private readonly DbContextOptions<AppDbContext>? _options;

    public TeaLotRepository()
    {
    }

    public TeaLotRepository(DbContextOptions<AppDbContext> options)
    {
      _options = options;
    }

    public async Task<List<TeaLot>> GetAllAsync()
    {
      using var db = _options != null ? new AppDbContext(_options) : new AppDbContext();
      return await db.TeaLots
        .Include(t => t.ProcessEvents)
        .Include(t => t.Shipments)
        .OrderByDescending(t => t.HarvestDate)
        .ToListAsync();
    }

    public async Task<List<TeaLot>> SearchAsync(string? query)
    {
      using var db = _options != null ? new AppDbContext(_options) : new AppDbContext();
      var q = db.TeaLots.AsQueryable();

      if (!string.IsNullOrWhiteSpace(query))
      {
        var trimmed = query.Trim();
        q = q.Where(t => t.LotCode.Contains(trimmed) || t.Origin.Contains(trimmed));
      }

      return await q
        .Include(t => t.ProcessEvents)
        .Include(t => t.Shipments)
        .OrderByDescending(t => t.HarvestDate)
        .ToListAsync();
    }

    public async Task<TeaLot?> GetByIdAsync(int id)
    {
      using var db = _options != null ? new AppDbContext(_options) : new AppDbContext();
      return await db.TeaLots
        .Include(t => t.ProcessEvents)
        .Include(t => t.Shipments)
        .FirstOrDefaultAsync(t => t.Id == id);
    }
  }
}

