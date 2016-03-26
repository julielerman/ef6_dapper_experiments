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
    private readonly int _iterations = 25;


    public EFTests() {
      //warm up EF
      using (var context = new DapperDesignerContext()) {
        context.Designers.Find(1);
        context.Clients.Find(1);
      }
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,Track")]
    public void GetAllDesigners_Tracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < _iterations; i++) {
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

    [TestMethod,TestCategory("EF"),TestCategory("EF,NoTrack")]
    public void GetAllDesigners_AsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < _iterations; i++) {
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

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetAllDesigners_ViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = "select * from DapperDesigners";

      for (int i = 0; i < _iterations; i++) {
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

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,Track")]
    public void GetAllDesigners_ViaRawSqlTracked() {
      List<long> times = new List<long>();
      var sql = "select * from DapperDesigners";
      for (int i = 0; i < _iterations; i++) {
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

    [TestMethod, TestCategory("EF"), TestCategory("EF,NoTrack")]
    public void GetAllDesignersWithProducts_AsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < _iterations; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.Include(d => d.Products).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithProductsAsNoTracking");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");
      Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetAllDesignersWithProducts_ViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = @"select * from DapperDesigners D 
                LEFT OUTER JOIN Products P
                ON P.DapperDesignerId = D.Id";
      var designers = new List<DapperDesigner>();
      for (int i = 0; i < _iterations; i++) {
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
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithProductsViaRawSqlNotTracked");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.AreNotEqual(0, designers.Select(d => d.Products).Count());
    }

    [TestMethod, TestCategory("EF"), TestCategory("EF,Track")]
    public void GetAllDesignersWithContact_Tracking() {
      List<long> times = new List<long>();
      for (int i = 0; i < _iterations; i++) {
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
    public void GetAllDesignersWithContact_AsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < _iterations; i++) {
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

    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetAllDesignersWithContact_ViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = @"select * from DapperDesigners D 
                LEFT OUTER JOIN ContactInfoes C
                ON C.Id = D.Id";
      var designers = new List<DapperDesigner>();
      for (int i = 0; i < _iterations; i++) {
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
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithContact_ViaRawSqlNotTracked");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.AreNotEqual(0, designers.Select(d => d.Clients).Count());
    }


    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetAllDesignersWithClients_ViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = @"SELECT D.*,C.*
                  FROM DapperDesigners D  
                  LEFT OUTER JOIN DapperDesignerClients dc on (D.Id = dc.DapperDesigner_Id)
                  LEFT OUTER JOIN Clients C on (dc.Client_Id = C.Id);";
      var designers = new List<DapperDesigner>();
      for (int i = 0; i < _iterations; i++) {
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


    [TestMethod, TestCategory("EF"), TestCategory("EF,NoTrack")]
    public void GetAllDesignersWithClients_AsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < _iterations; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          var designers = context.Designers.Include(d => d.Clients).AsNoTracking().ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetAllDesignersWithClientsAsNoTracking");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");
      Assert.IsTrue(true);
    }



    [TestMethod, TestCategory("EF"), TestCategory("EF,Track")]
    public void GetAllDesignersWithContactsAndClients_Tracking() {
      List<long> times = new List<long>();
      for (int i = 0; i < _iterations; i++) {
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


    [TestMethod, TestCategory("EF"), TestCategory("EF,NoTrack")]
    public void GetAllDesignersWithContactsAndClients_AsNoTracking() {
      List<long> times = new List<long>();

      for (int i = 0; i < _iterations; i++) {
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

    [TestMethod, TestCategory("EF"), TestCategory("EF,NoTrack")]
    public void GetProjectedDesigners() {
      using (var context = new DapperDesignerContext()) {
        var miniDesigner =
          context.Designers.AsNoTracking()
          .Select(d => new MiniDesigner {
            Id = d.Id, Name = d.LabelName, FoundedBy = d.Founder
            }).FirstOrDefault();
         Assert.AreNotEqual(" ", miniDesigner.Name);
      }


    }
    [TestMethod, TestCategory("EF"), TestCategory("EF,RawSQL,NoTrack")]
    public void GetProjectedDesigners_ViaRawSqlNotTracked() {
      List<long> times = new List<long>();
      var sql = "select LabelName as Name,id from DapperDesigners";
      var designers = new List<MiniDesigner>();
      for (int i = 0; i < _iterations; i++) {
        using (var context = new DapperDesignerContext()) {
          _sw.Reset();
          _sw.Start();
          designers = context.Database.SqlQuery<MiniDesigner>(sql).ToList();
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _trackedObjects = context.ChangeTracker.Entries().Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "EF: GetProjectedDesigners_ViaRawSqlNotTracked");
      Console.WriteLine($"Latest Tracked Objects:{_trackedObjects}");

      Assert.AreNotEqual(0, designers.Count());
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