using DapperDesigners.Data.EF6;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTests.cs
{
  [TestClass]
  public class EFTests
  {
    private System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();

    public EFTests() {
      //warm up EF
      using (var context = new DapperDesignerContext()) {
        context.Designers.Find(1);
        context.Clients.Find(1);
      }
    }

    [TestMethod]
    public void GetAllDesignersAsNoTracking() {
      List<long> times = new List<long>();

      using (var context = new DapperDesignerContext()) {
        for (int i = 0; i < 25; i++) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
        }
        Console.WriteLine($"Tracked Object:{context.ChangeTracker.Entries().Count()}");
      }
      var analyzer = new TimeAnalyzer(times);
      Output(times, analyzer, "EF: GetAllDesigners");
      Assert.IsTrue(true);
    }

    [TestMethod]
    public void GetAllDesignersTracking() {
      List<long> times = new List<long>();

      using (var context = new DapperDesignerContext()) {
        for (int i = 0; i < 25; i++) {
          _sw.Reset();
          _sw.Start();
            var designers = context.Designers.ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
        }
        Console.WriteLine($"Tracked Object:{context.ChangeTracker.Entries().Count()}");

      }
      var analyzer = new TimeAnalyzer(times);
      Output(times, analyzer, "EF: GetAllDesigners");
      Assert.IsTrue(true);
    }

    private void Output(List<long> times, TimeAnalyzer analyzer, string source) {
      times.ForEach(t => Console.WriteLine(t));
      Console.WriteLine(source);
      Console.WriteLine($"Total: {analyzer.Cumulative}");
      Console.WriteLine($"Fastest: {analyzer.Fastest}");
      Console.WriteLine($"Slowest: {analyzer.Slowest}");
      Console.WriteLine($"Average without Fastest or Slowest: {analyzer.AverageWithOutFastestOrSlowest}");
    }
  }

  internal class TimeAnalyzer
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