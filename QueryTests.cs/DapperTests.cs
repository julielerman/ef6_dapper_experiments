using Dapper;
using Dapper.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTests
{
  [TestClass]
  public class DapperTests
  {
    private System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();
    private int _retrievedObjects;

    [TestMethod]
    public void CanGetAllDesigners() {
      using (var conn = Utils.CreateOpenConnection()) {
        var designers = conn.Query<DapperDesigner>("select Top 1 * from DapperDesigners");
        Assert.AreEqual(1, designers.Count());
      }
    }
    [TestMethod]
    public void GetAllDesigners() {
      List<long> times = new List<long>();
      for (int i = 0; i < 25; i++) {
        using (var conn = Utils.CreateOpenConnection()) {
          _sw.Reset();
          _sw.Start();
          var designers = conn.Query<DapperDesigner>("select * from DapperDesigners");
          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _retrievedObjects = designers.Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "Dapper: GetAllDesignersRawSql");
      Console.WriteLine($"Latest Retrieved Object Count:{_retrievedObjects}");

      Assert.IsTrue(true);
    }

    [TestMethod]
    public void CanReuseOpenConnection() {
      var connStates = new List<string>();
      using (var conn = Utils.CreateOpenConnection()) {
        for (int i = 0; i < 3; i++) {
          connStates.Add(conn.State.ToString());
          var designers = conn.Query<DapperDesigner>("select top 1 * from DapperDesigners");
        }
      }
      CollectionAssert.AreEqual(new[] { "Open", "Open", "Open" }, connStates.ToArray());
    }

    [TestMethod]
    public void GetAllDesignersWithProducts() {
      var sql = @"select * from DapperDesigners D 
                JOIN Products P
                ON P.DapperDesignerId = D.Id";
      List<long> times = new List<long>();
      IEnumerable<DapperDesigner> designers=new List<DapperDesigner>();
      for (int i = 0; i < 25; i++) {
        using (var conn = Utils.CreateOpenConnection()) {
          _sw.Reset();
          _sw.Start();
           designers= conn.Query<DapperDesigner, Product,DapperDesigner>(sql,
      (designer, product) => { designer.Products.Add(product); return designer; });

          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _retrievedObjects = designers.Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "Dapper: GetAllDesignersRawSql");
      Console.WriteLine($"Latest Retrieved Object Count:{_retrievedObjects}");
      Assert.AreNotEqual(0,designers.Select(d => d.Products).Count());
    }
    [TestMethod]
    public void GetAllDesignersWithClients() {
      var sql = @"SELECT D.Id, d.LabelName,C.Id, C.Name
                  FROM DapperDesigners D  
                  LEFT OUTER JOIN DapperDesignerClients dc on (D.Id = dc.DapperDesigner_Id)
                  LEFT OUTER JOIN Clients C on (dc.Client_Id = C.Id);";
      List<long> times = new List<long>();
      IEnumerable<DapperDesigner> designers = new List<DapperDesigner>();
      for (int i = 0; i < 25; i++) {
        using (var conn = Utils.CreateOpenConnection()) {
          _sw.Reset();
          _sw.Start();
          designers = conn.Query<DapperDesigner, Client, DapperDesigner>
            (sql,(designer, client) 
            => { designer.Clients.Add(client); return designer; }
            );

          _sw.Stop();
          times.Add(_sw.ElapsedMilliseconds);
          _retrievedObjects = designers.Count();
        }
      }
      var analyzer = new TimeAnalyzer(times);
      Utils.Output(times, analyzer, "Dapper: GetAllDesignersWithClients");
      Console.WriteLine($"Latest Retrieved Object Count:{_retrievedObjects}");
      Assert.AreNotEqual(0, designers.Select(d => d.Clients).Count());
    }

    [TestMethod]
    public void GetFilteredDesigners() { }

    [TestMethod]
    public void GetFilteredAndOrderedDesigners() { }
  }
}