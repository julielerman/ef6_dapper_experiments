using System.Collections.Generic;
using System.Linq;

namespace QueryTests.Helpers
{
  public class TimeAnalyzer
  {
    private List<long> _times;

    public TimeAnalyzer(List<long> times) {
      _times = times;
      CalculateAllProperties();
    }

    public long Cumulative { get; set; }
    public long First { get; set; }
    public long Last { get; set; }
    public long Fastest { get; set; }
    public long Slowest { get; set; }
    public long AverageWithOutFastestOrSlowest { get; set; }

    private void CalculateAllProperties() {
      Cumulative = _times.Sum();
      First = _times.FirstOrDefault();
      Last = _times.LastOrDefault();
      Fastest = _times.Min();
      Slowest = _times.Max();
      AverageWithOutFastestOrSlowest = (Cumulative - Fastest - Slowest) / _times.Count;
    }
  }
}