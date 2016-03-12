using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DapperDesigners.Data.EF6
{
  public class ContextConfiguration : DbConfiguration
  {
    public ContextConfiguration() {
      SetDefaultConnectionFactory(new LocalDbConnectionFactory("mssqllocaldb"));
    }
  }
}