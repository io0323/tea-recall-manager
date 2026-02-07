using System.Collections.Generic;
using System.Threading.Tasks;
using TeaRecallManager.Core.Models;

namespace TeaRecallManager.Core.Services
{
  /* 回収影響分析サービスのインタフェース */
  public interface IRecallAnalysisService
  {
    Task<RecallAnalysisResult> AnalyzeAsync(int seedTeaLotId);
  }
}

