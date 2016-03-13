using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QueryTests.Helpers
{
  public class Utils
  {
    public static void Output(List<long> times, TimeAnalyzer analyzer, string source) {
      times.ForEach(t => Console.WriteLine(t));
      Console.WriteLine(source);
      Console.WriteLine($"Total: {analyzer.Cumulative}");
      Console.WriteLine($"Fastest: {analyzer.Fastest}");
      Console.WriteLine($"Slowest: {analyzer.Slowest}");
      Console.WriteLine($"Average without Fastest or Slowest: {analyzer.AverageWithOutFastestOrSlowest}");
    }
    public static SqlConnection CreateOpenConnection() {
      string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DapperDesigns;Integrated Security=True;Connect Timeout=30;";
      var conn = new SqlConnection(connString);
      conn.Open();
      return conn;
    }
  }
}