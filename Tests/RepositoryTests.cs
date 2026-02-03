using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TeaRecallManager.Data;
using TeaRecallManager.Core.Models;
using TeaRecallManager.Repositories;
using Xunit;

namespace TeaRecallManager.Tests
{
  /* Repository の基本動作を検証する単体テスト */
  public class RepositoryTests
  {
    private static DbContextOptions<AppDbContext> CreateInMemoryOptions(SqliteConnection conn)
    {
      return new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(conn)
        .Options;
    }

    [Fact]
    public async Task SearchAsync_returns_inserted_lot()
    {
      using var conn = new SqliteConnection("DataSource=:memory:");
      conn.Open();
      var options = CreateInMemoryOptions(conn);

      // Arrange - seed one lot
      using (var db = new AppDbContext(options))
      {
        db.Database.EnsureCreated();
        db.TeaLots.Add(new TeaLot { LotCode = "TEST-001", Origin = "UnitTest Farm", Variety = "Sencha", HarvestDate = System.DateTime.Today, Status = TeaLotStatus.Normal });
        db.SaveChanges();
      }

      // Act
      var repo = new TeaLotRepository(options);
      var results = await repo.SearchAsync("TEST-001");

      // Assert
      Assert.Single(results);
      Assert.Equal("TEST-001", results.First().LotCode);
    }

    [Fact]
    public async Task GetByIdAsync_returns_correct_lot()
    {
      using var conn = new SqliteConnection("DataSource=:memory:");
      conn.Open();
      var options = CreateInMemoryOptions(conn);

      int id;
      using (var db = new AppDbContext(options))
      {
        db.Database.EnsureCreated();
        var lot = new TeaLot { LotCode = "TEST-002", Origin = "UnitTest Farm", Variety = "Gyokuro", HarvestDate = System.DateTime.Today, Status = TeaLotStatus.Normal };
        db.TeaLots.Add(lot);
        db.SaveChanges();
        id = lot.Id;
      }

      var repo = new TeaLotRepository(options);
      var got = await repo.GetByIdAsync(id);
      Assert.NotNull(got);
      Assert.Equal("TEST-002", got!.LotCode);
    }
  }
}

