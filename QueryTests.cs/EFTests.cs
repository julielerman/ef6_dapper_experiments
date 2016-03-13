using Dapper.Domain;
using DapperDesigners.Data.EF6;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryTests.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace QueryTests.cs
{
  [TestClass]
  public class EFTests
  {
    private System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();
    private int _trackedObjects;

    public EFTests() {
      //warm up EF
      using (var context = new DapperDesignerContext()) {
        context.Designers.Find(1);
        context.Clients.Find(1);
      }
    }

    [TestMethod,TestCategory("EF"),TestCategory("EF,NoTrack")]
    public void GetAllDesignersAsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesigners");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,Track")]
    public void GetAllDesignersTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesigners");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,NoTrack")]
    public void GetAllDesignersWithContactAsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.Include(d => d.ContactInfo).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithContactAsNoTracking");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");
      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,Track")]
    public void GetAllDesignersWithContactTracking() {
      List<long> times = new List<long>();
      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.Include(d => d.ContactInfo).ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);

      Utils.Output(times, analyzer, "EF: GetAllDesignersWithContactTracking");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,NoTrack")]
    public void GetAllDesignersWithContactsAndClientsAsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.Include(d => d.ContactInfo).Include(d => d.Clients).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);

          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithContactsAndClientsAsNoTracking");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");
      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,Track")]
    public void GetAllDesignersWithContactsAndClientsTracking() {
      List<long> times = new List<long>();
      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.Include(d => d.ContactInfo).Include(d => d.Clients).ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithContactsAndClientsTracking");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");
      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,Track")]
    public void GetAllDesignersViaRawSqlTracked() {
      List<long> times = new List<long>();
      var sql = "select * from DapperDesigners";
      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.SqlQuery(sql).ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersRawSql");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetAllDesignersViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = "select * from DapperDesigners";

      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.SqlQuery(sql).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersRawSql");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetAllDesignersWithClientsViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = @"SELECT D.*,C.*
                  FROM DapperDesigners D  
                  LEFT OUTER JOIN DapperDesignerClients dc on (D.Id = dc.DapperDesigner_Id)
                  LEFT OUTER JOIN Clients C on (dc.Client_Id = C.Id);";
      var designers=new List<DapperDesigner>();
      for (int i = 0; i < 25; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
           designers = context.Designers.SqlQuery(sql).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithClientsViaRawSqlNotTracked");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.AreNotEqual(0, designers.Select(d => d.Clients).Count());
    }


    [TestMethod]
    public void CanReuseOpenConnection() {
      SqlConnection conn = Utils.CreateOpenConnection();
      var connStates = new List<string>();
      for (int i = 0; i < 3; i++) {
        connStates.Add(conn.State.ToString());
        using (var context = new DapperDesignerContext(conn, contextOwnsConnection: false)) {
          var designers = context.Designers.SqlQuery("select top 1 from DapperDesigners");
        }
      }
      CollectionAssert.AreEqual(new[] { "Open", "Open", "Open" }, connStates.ToArray());
    }
  }
}