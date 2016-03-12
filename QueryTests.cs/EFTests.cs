using AdvWorks.Data.EF6;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace QueryTests.cs
{
  [TestClass]
  public class EFTests
  {
    private System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();
    int _trackedObjects;

    public EFTests() {
      //warm up EF
      using (var context = new AdventureWorksModel()) {
        context.Customers.FirstOrDefault();
        context.SalesOrderHeaders.FirstOrDefault();
      }
    }

    [TestMethod]
    public void GetAllCustomersAsNoTracking() {
      List<long> times = new List<long>();

      using (var context = new AdventureWorksModel()) {
        for (int i = 0; i < 25; i++) {
          _sw.Reset();
          _sw.Start();
          var customers = context.Customers.AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
        }
        _trackedObjects = context.ChangeTracker.Entries().Count();

      }
      var analyzer = new TimeAnalyzer(times);
      Output(times, analyzer, "EF: GetAllCustomersAsNoTracking");
      Console.WriteLine($"Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod]
    public void GetAllCustomersTracking() {
      List<long> times = new List<long>();

      using (var context = new AdventureWorksModel()) {
        for (int i = 0; i < 25; i++) {
          _sw.Reset();
          _sw.Start();
          var customers = context.Customers.ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
        }
        _trackedObjects = context.ChangeTracker.Entries().Count();

      }
      var analyzer = new TimeAnalyzer(times);

      Output(times, analyzer, "EF: GetAllCustomersTracking");
      Console.WriteLine($"Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod]
    public void GetAllDesignersWithContactAsNoTracking() {
      List<long> times = new List<long>();
   

      using (var context = new AdventureWorksModel()) {
        for (int i = 0; i < 25; i++) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Customers.Include(d => d.SalesOrderHeaders).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
        }
        _trackedObjects = context.ChangeTracker.Entries().Count();

      }
      var analyzer = new TimeAnalyzer(times);
      Output(times, analyzer, "EF: GetAllDesignersWithContactAsNoTracking");
      Console.WriteLine($"Tracked Objects:{_trackedObjects}");
      Assert.IsTrue(true);
    }

    [TestMethod]
    public void GetAllDesignersWithContactTracking() {
      List<long> times = new List<long>();
       using (var context = new AdventureWorksModel()) {
        for (int i = 0; i < 25; i++) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Customers.Include(d => d.SalesOrderHeaders).ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
        }
        _trackedObjects = context.ChangeTracker.Entries().Count();

      }
      var analyzer = new TimeAnalyzer(times);

      Output(times, analyzer, "EF: GetAllDesignersWithContactTracking");
      Console.WriteLine($"Tracked Objects:{_trackedObjects}");

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